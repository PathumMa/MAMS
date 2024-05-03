using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
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
        public ActiveStatus Status { get; set; } = ActiveStatus.Active;

        public UserDetails UserDetails { get; set; }
        public DoctorDetails DoctorDetails { get; set; }
    }
}
