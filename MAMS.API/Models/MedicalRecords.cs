using System.ComponentModel.DataAnnotations;

namespace MAMS.API.Models
{
    public class MedicalRecords
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int User_PersonalId { get; set; }
        [Required]
        public int Doctor_PersonalId { get; set; }
        public DateTime Appointment_Date { get; set;}
        public string? Diagnosis { get; set; }
        public string? Treatment_plan { get; set; }
        public string? Prescription { get; set; }
        public DateTime CreatedDate { get; set;} = DateTime.Now;
    }
}
