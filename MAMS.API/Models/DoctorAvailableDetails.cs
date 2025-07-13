using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
{
    public class DoctorAvailableDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public string Available_Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
        public DateTime? Created_Date { get; set; } = DateTime.Now;
        public string? Created_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string? Modified_By { get; set; }

        public DoctorDetails DoctorDetails { get; set; }
        public List<Appointments> Appointments { get; set; }
    }
}
