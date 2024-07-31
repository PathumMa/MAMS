using static MAMS.API.Tools.Enums;

namespace MAMS.API.DTOs
{
    public class DoctorAvailabilityDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string Available_Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
    }
}
