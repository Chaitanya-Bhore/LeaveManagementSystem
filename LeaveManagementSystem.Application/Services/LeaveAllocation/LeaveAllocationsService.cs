using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.EntityFrameworkCore;
using LeaveAllocationEntity = LeaveManagementSystem.Data.LeaveAllocation;

namespace LeaveManagementSystem.Application.Services.LeaveAllocation
{
    [Authorize]
    public class LeaveAllocationsService(
        ApplicationDbContext _context,
        IUserService _userService,
        IMapper _mapper,
        IPeriodsService _periodsService)
        : ILeaveAllocationService
    {
        public async Task AllocateLeave(string employeeId)
        {
            // Get the leave types from the database
            var leaveTypes = await _context.LeaveTypes
                .Where(lt => !lt.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();

            // Get the current period based on the year
            var currentDate = DateTime.Now;
            var period = await _context.Periods
                .SingleAsync(p => p.EndDate.Year == currentDate.Year);

            var monthsRemaining = period.EndDate.Month - currentDate.Month;

            // foreach leave type, create an allocation entry for the employee
            foreach (var leaveType in leaveTypes)
            {
                var allocationExists = await AllocationExists(
                    employeeId,
                    period.Id,
                    leaveType.Id);

                if (allocationExists)
                {
                    continue;
                }

                var accuralRate = decimal.Divide(
                    leaveType.NumberOfDays,
                    12);

                var leaveAllocation = new LeaveAllocationEntity
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = (int)Math.Ceiling(
                        accuralRate * monthsRemaining)
                };

                _context.Add(leaveAllocation);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<LeaveAllocationEntity> GetCurrentAllocation(
            int leaveTypeId,
            string employeeId)
        {
            var period = await _periodsService.GetCurrentPeriod();

            var allocation = await _context.LeaveAllocations
                .FirstAsync(la =>
                    la.LeaveTypeId == leaveTypeId &&
                    la.EmployeeId == employeeId &&
                    la.PeriodId == period.Id);

            return allocation;
        }

        public async Task<List<LeaveAllocationEntity>> GetAllocations(
            string? userId)
        {
            string employeeId = string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var user = await _userService.GetLoggedInUser();

                if (user == null)
                {
                    throw new ApplicationException("User not found.");
                }

                employeeId = user.Id;
            }
            else
            {
                employeeId = userId;
            }

            var currentDate = DateTime.Now;

            var period = await _context.Periods
                .SingleAsync(p => p.EndDate.Year == currentDate.Year);

            // Implementation for getting leave allocations for a specific employee
            var leaveAllocations = await _context.LeaveAllocations
                .Include(la => la.LeaveType)
                .Include(la => la.Period)
                .Where(la =>
                    la.EmployeeId == employeeId &&
                    la.PeriodId == period.Id)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(
            string? userId)
        {
            var user = string.IsNullOrEmpty(userId)
                ? await _userService.GetLoggedInUser()
                : await _userService.GetUserById(userId);

            var allocations = await GetAllocations(userId);

            var allocationVMList =
                _mapper.Map<List<LeaveAllocationVM>>(allocations);

            var leaveTypes = await _context.LeaveTypes.CountAsync();

            var employeeVM = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                userId = user.Id,
                LeaveAllocations = allocationVMList,
                IsCompletedAllocation = allocations.Count == leaveTypes
            };

            return employeeVM;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userService.GetEmployees();

            var employees =
                _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users);

            return employees;
        }

        private async Task<bool> AllocationExists(
            string? userId,
            int periodId,
            int leaveTypeId)
        {
            var exists = await _context.LeaveAllocations
                .AnyAsync(la =>
                    la.EmployeeId == userId &&
                    la.PeriodId == periodId &&
                    la.LeaveTypeId == leaveTypeId);

            return exists;
        }

        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(
            int allocationId)
        {
            var allocation = await _context.LeaveAllocations
                .Include(la => la.LeaveType)
                .Include(la => la.Period)
                .Include(la => la.Employee)
                .FirstOrDefaultAsync(la => la.Id == allocationId);

            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;
        }

        public async Task EditAllocation(
            LeaveAllocationEditVM allocationEditVM)
        {
            var leaveAllocation = await _context.LeaveAllocations
                .FirstOrDefaultAsync(la =>
                    la.Id == allocationEditVM.Id);

            if (leaveAllocation == null)
            {
                throw new ApplicationException(
                    "Leave allocation not found.");
            }

            leaveAllocation.Days = allocationEditVM.Days;

            _context.Update(leaveAllocation);

            await _context.SaveChangesAsync();
        }
    }
}