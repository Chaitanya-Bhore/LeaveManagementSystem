namespace LeaveManagementSystem.Application.Models.LeaveAllocations
{
    public class EmployeeListVM
    {
        public string userId { get; set; } = string.Empty;

        [Display(Name = "First Name")]
        public string Firstname { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string Lastname { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}