using System.ComponentModel.DataAnnotations;
using static MAMS.Services.Enums;

namespace MAMS.Models
{
    public class DoctorAvailableDetails
    {
        [Required]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string Available_Day { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;

        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }
    }
}
