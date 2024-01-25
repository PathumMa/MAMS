using System.ComponentModel.DataAnnotations;

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
        public int Status { get; set; } = 1;

        public UserDetails UserDetails { get; set; }
        public DoctorDetails DoctorDetails { get; set; }
    }
}
