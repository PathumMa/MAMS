using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MAMS.API.Models
{
    public class DoctorDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SuserId { get; set; }
        public string UserTitle { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string? Middle_Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public DateTime Birth_Date { get; set; }
        public string Gender { get; set; }
        public string Blood_Type { get; set; }
        [Required]
        public string Personal_Id { get; set; }
        public string PersonalId_Type { get; set; }
        public string? MedicalCouncilRegistrationNumber { get; set; }
        [Required]
        public int Specialization_Id { get; set; }

        [ForeignKey("Specialization_Id")]
        public Specializations Specialization { get; set; }
        public string? Hospital_Affiliation { get; set; }
        public decimal? Doctor_Fee { get; set; }
        public int Auth_Status { get; set; } = 0;
        public DateTime Created_Date { get; set; } = DateTime.Now;
        public string? Created_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string? Modified_By { get; set; }

        public Suser Suser { get; set; }
        public ICollection<DoctorAvailableDetails> AvailableDetails { get; set; }
        public List<Appointments> Appointments { get; set; }
        public List<MedicalRecords> MedicalRecords { get; set; }
        
    }
}
