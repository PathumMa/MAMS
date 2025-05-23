using System.ComponentModel.DataAnnotations;
using MAMS.Models;

namespace MAMS.Models
{
    public class PatientDetails
    {
        [Key]
        public int Id { get; set; }
        public int? RegisteredUserId { get; set; }
        public string UserTitle { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        public string PersonalId { get; set; }
        public string PersonalIdType { get; set; }
        public int Appointment_Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public Appointments Appointments { get; set; }
    }
}
