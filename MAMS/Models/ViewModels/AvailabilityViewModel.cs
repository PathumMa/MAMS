namespace MAMS.Models.ViewModels
{
    public class AvailabilityViewModel
    {
        public DayOfWeek? Available_Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
