using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Models.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveAllocation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    public class LeaveRequestsService(
        IMapper _mapper,
        UserManager<ApplicationUser> _userManager,
        IHttpContextAccessor _httpContextAccessor,
        ApplicationDbContext _context,
        ILeaveAllocationService _leaveAllocationsService) : ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

            leaveRequest.LeaveRequestStatusId =
                (int)LeaveRequestStatusEnum.Cancelled;

            // restore allocation days based on request
            await UpdateAllocationDays(leaveRequest, false);

            await _context.SaveChangesAsync();
        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            // map data to leave request from the view model
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            // get logged in employee id and assign it to the leave request
            var user = await _userManager.GetUserAsync(
                _httpContextAccessor.HttpContext.User);

            leaveRequest.EmployeeId = user.Id;

            // set the leave request status to pending
            leaveRequest.LeaveRequestStatusId =
                (int)LeaveRequestStatusEnum.Pending;

            // save the leave request to the database
            _context.LeaveRequests.Add(leaveRequest);

            // deduct allocation days from the employee's leave allocation
            await UpdateAllocationDays(leaveRequest, true);

            await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userManager.GetUserAsync(
                _httpContextAccessor.HttpContext.User);

            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                NumberOfDays =
                    q.EndDate.DayNumber -
                    q.StartDate.DayNumber,
                LeaveRequestStatus =
                    (LeaveRequestStatusEnum)q.LeaveRequestStatusId
            }).ToList();

            return model;
        }

        public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .ToListAsync();

            var approvedLeaveRequestsCount =
                leaveRequests.Count(q =>
                    q.LeaveRequestStatusId ==
                    (int)LeaveRequestStatusEnum.Approved);

            var pendingLeaveRequestsCount =
                leaveRequests.Count(q =>
                    q.LeaveRequestStatusId ==
                    (int)LeaveRequestStatusEnum.Pending);

            var rejectedLeaveRequestsCount =
                leaveRequests.Count(q =>
                    q.LeaveRequestStatusId ==
                    (int)LeaveRequestStatusEnum.Rejected);

            var leaveRequestModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                NumberOfDays =
                    q.EndDate.DayNumber -
                    q.StartDate.DayNumber,
                LeaveRequestStatus =
                    (LeaveRequestStatusEnum)q.LeaveRequestStatusId
            }).ToList();

            var model = new EmployeeLeaveRequestListVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = approvedLeaveRequestsCount,
                PendingRequests = pendingLeaveRequestsCount,
                RejectedRequests = rejectedLeaveRequestsCount,
                LeaveRequests = leaveRequestModels
            };

            return model;
        }

        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {

            var user = await _userManager.GetUserAsync(
                _httpContextAccessor.HttpContext.User);
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q =>
                q.EndDate.Year == currentDate.Year);

            var numberOfDays =
                model.EndDate.DayNumber -
                model.StartDate.DayNumber;

            var allocationtoDeduct = await _context.LeaveAllocations
                .FirstAsync(q =>
                    q.LeaveTypeId == model.LeaveTypeId
                    &&
                    q.EmployeeId == user.Id
                    &&
                    q.PeriodId == period.Id);

            return allocationtoDeduct.Days < numberOfDays;
        }

        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var user = await _userManager.GetUserAsync(
                _httpContextAccessor.HttpContext?.User);

            var leaveRequest =
                await _context.LeaveRequests.FindAsync(leaveRequestId);

            leaveRequest.LeaveRequestStatusId =
                (int)(approved
                    ? LeaveRequestStatusEnum.Approved
                    : LeaveRequestStatusEnum.Rejected);

            leaveRequest.ReviewerId = user.Id;

            if (!approved)
            {
                await UpdateAllocationDays(leaveRequest, false);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstAsync(q => q.Id == id);

            var user = await _userManager.FindByIdAsync(
                leaveRequest.EmployeeId);

            var model = new ReviewLeaveRequestVM
            {
                Id = leaveRequest.Id,
                LeaveType = leaveRequest.LeaveType.Name,
                RequestComments = leaveRequest.RequestComments,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays =
                    leaveRequest.EndDate.DayNumber -
                    leaveRequest.StartDate.DayNumber,
                LeaveRequestStatus =
                    (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,

                Employee = new EmployeeListVM
                {
                    userId = leaveRequest.EmployeeId,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Email = user.Email
                }
            };

            return model;
        }

        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationsService.GetCurrentAllocation
                (leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
            var numberOfDays = CalculateDays(
                leaveRequest.StartDate,
                leaveRequest.EndDate);

            if (deductDays)
            {
                allocation.Days -= numberOfDays;
            }
            else
            {
                allocation.Days += numberOfDays;
            }

            _context.Entry(allocation).State = EntityState.Modified;
        }

        private int CalculateDays(DateOnly start, DateOnly end)
        {
            return (end.DayNumber - start.DayNumber) + 1;
        }
    }
}