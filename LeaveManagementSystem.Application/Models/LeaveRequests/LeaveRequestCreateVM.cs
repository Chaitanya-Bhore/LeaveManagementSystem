using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Application.Models.LeaveRequests
{
    public class LeaveRequestCreateVM : IValidatableObject
    {
        [Display(Name = "Start Date")]
        [Required]
        public DateOnly StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required]
        public DateOnly EndDate { get; set; }
        [Required]

        [Display(Name = "Desired Leave Type")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "Additional Information")]
        [StringLength(300, ErrorMessage = "Comments cannot exceed 300 characters")]
        public string RequestComments { get; set; } = string.Empty;

        public SelectList? LeaveTypes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                yield return new ValidationResult("Start Date cannot be after End Date", new[] { nameof(StartDate), nameof(EndDate) });
            }
        }
    }
}