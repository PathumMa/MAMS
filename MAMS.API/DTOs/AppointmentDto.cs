using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.DTOs
{
    public class AppointmentDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User_PersonalId { get; set; }
        [Required]
        public int Doctor_Id { get; set; }
        public DateTime Appointment_Date { get; set; }
        public int Appoinment_number { get; set; }
        public ActiveStatus Status { get; set; } = ActiveStatus.Active;
    }
}
