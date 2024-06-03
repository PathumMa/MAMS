using System.ComponentModel.DataAnnotations;
using static MAMS.Services.Enums;

namespace MAMS.Models
{
    public class DoctorAvailableDetails
    {
        [Required]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string Day { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }
    }
}
