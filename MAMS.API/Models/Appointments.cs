using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("Doctor_Id")]
        public DoctorDetails Doctor { get; set; }

        [Required]
        public int Availability_Id { get; set; }

        [Required]
        public DateTime Appointment_Date { get; set; }

        public int Appoinment_number { get; set; }

        [Required]
        public int PatientDetails_Id { get; set; }

        [ForeignKey("PatientDetails_Id")]
        public PatientDetails PatientDetails { get; set; }

        public ActiveStatus Status { get; set; } = ActiveStatus.Active;

        public Transactions Transaction { get; set; }
    }


}
