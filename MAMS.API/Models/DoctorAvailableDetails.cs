using System.ComponentModel.DataAnnotations;

namespace MAMS.API.Models
{
    public class DoctorAvailableDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SuserDetailsId { get; set; }
        public string? AvailableDay { get; set; }
        public string AvailableTime { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public SuserDetails SuserDetails { get; set; }
    }
}
