using System.ComponentModel.DataAnnotations;

namespace MAMS.Models
{
    public class Suser
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePictureURL { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int IsActive { get; set; } = 1;

        public SuserDetails SuserDetails { get; set; }
        public UserDetails UserDetails { get; set; }
        public DoctorDetails DoctorDetails { get; set; }
    }
}
