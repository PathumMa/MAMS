using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
{
    public class Appointments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User_PersonalId { get; set; }
        [Required]
        public int Doctor_Id { get; set; }
        [Required]
        public int Availability_Id { get; set; }
        public DateTime Appointment_Date { get; set; }
        public int Appoinment_number { get; set; }
        public ActiveStatus Status { get; set; } = ActiveStatus.Active;

        public Transactions? Transactions { get; set; }
        public PatientDetails PatientDetails { get; set; }
    }
}
