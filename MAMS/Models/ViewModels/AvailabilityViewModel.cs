using static MAMS.Services.Enums;

namespace MAMS.Models.ViewModels
{
    public class AvailabilityViewModel
    {
        public Days Available_Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
