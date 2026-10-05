namespace LeaveManagementSystem.Application.Models.Periods
{
    public class PeriodVM
    {
        public int Id { get; set; }

        public object Name { get; internal set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
