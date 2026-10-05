using LeaveManagementSystem.Application.Models.LeaveTypes;

namespace LeaveManagementSystem.Application.Services.LeaveTypes;

public interface ILeaveTypesService
{
    Task<bool> CheckIfLeaveTypeNameExists(string name);
    Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leavetypeEdit);
    Task Create(LeaveTypeCreateVM model);
    Task<bool> DaysExceedMaximum(int leaveTypeId, int days);
    Task Edit(LeaveTypeEditVM model);
    Task<T?> Get<T>(int id) where T : class;
    Task<List<LeaveTypeReadOnlyVM>> GetAllLeaveTypes();
    bool LeaveTypeExists(int id);
    Task Remove(int id);
}