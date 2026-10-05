using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveAllocationEntity = LeaveManagementSystem.Data.LeaveAllocation;

namespace LeaveManagementSystem.Application.Services.LeaveAllocation
{
    public interface ILeaveAllocationService
    {
        Task AllocateLeave(string EmployeeId);

        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);

        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId);

        Task<List<EmployeeListVM>> GetEmployees();

        Task EditAllocation(LeaveAllocationEditVM allocationEditVM);

        Task<LeaveAllocationEntity> GetCurrentAllocation(int leaveTypeId, string employeeId);
    }
}