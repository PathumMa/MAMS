using System.ComponentModel.DataAnnotations;

namespace MAMS.Models
{
    public class DoctorAvailableDetails
    {
        [Required]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DayOfWeek Day { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }
    }
}
