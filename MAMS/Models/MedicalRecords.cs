using System.ComponentModel.DataAnnotations;

namespace MAMS.Models
{
    public class MedicalRecords
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public DateTime Appointment_Date { get; set;}
        public string? Diagnosis { get; set; }
        public string? Treatment_plan { get; set; }
        public string? Prescription { get; set; }
        public DateTime CreatedDate { get; set;} = DateTime.Now;

        public UserDetails UserDetails { get; set; }
        public DoctorDetails DoctorDetails { get; set; }
    }
}
