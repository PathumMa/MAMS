using System.ComponentModel.DataAnnotations;

namespace MAMS.API.Models
{
    public class DoctorAvailableDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public DayOfWeek? Available_Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime Created_Date { get; set; } = DateTime.Now;

        public DoctorDetails DoctorDetails { get; set; }
    }
}
