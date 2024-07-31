using System.ComponentModel.DataAnnotations;
using static MAMS.Services.Enums;

namespace MAMS.Models
{
    public class Appoinments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public DateTime Appoinment_Date { get; set; }
        public ActiveStatus Status { get; set; } = ActiveStatus.Active;

        public UserDetails UserDetails { get; set; }
        public DoctorDetails DoctorDetails { get; set; }
    }
}
