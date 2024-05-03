using System.ComponentModel.DataAnnotations;

namespace MAMS.Models
{
    public class DoctorAvailableDetails
    {
        public DayOfWeek Day { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }
    }
}
