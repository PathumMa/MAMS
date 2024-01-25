using System.ComponentModel.DataAnnotations;

namespace MAMS.API.Models
{
    public class DoctorAvailableDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public string? Available_Day { get; set; }
        public string? Available_Time { get; set; }
        public DateTime Created_Date { get; set; } = DateTime.Now;

        public DoctorDetails DoctorDetails { get; set; }
    }
}
