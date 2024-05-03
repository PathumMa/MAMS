namespace MAMS.API.DTOs
{
    public class DoctorAvailabilityDto
    {
        public DayOfWeek? Available_Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
