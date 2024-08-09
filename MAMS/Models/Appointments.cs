using System.ComponentModel.DataAnnotations;
using static MAMS.Services.Enums;

namespace MAMS.Models
{
    public class Appointments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User_PersonalId { get; set; }
        [Required]
        public int Doctor_Id { get; set; }
        public DateTime Appoinment_Date { get; set; }
        public ActiveStatus Status { get; set; } = ActiveStatus.Active;

        public UserDetails UserDetails { get; set; }
        public DoctorDetails DoctorDetails { get; set; }
        public Transactions Transactions { get; set; }
    }
}
