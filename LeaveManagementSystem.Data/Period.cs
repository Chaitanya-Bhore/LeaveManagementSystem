namespace LeaveManagementSystem.Data
{
    public class Period : BaseEntity
    {

        public string Name { get; set; }
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }
    }
}
