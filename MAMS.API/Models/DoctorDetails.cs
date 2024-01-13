using System.ComponentModel.DataAnnotations;

namespace MAMS.API.Models
{
    public class DoctorDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SuserDetailsId { get; set; }
        public string Speciality { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public SuserDetails SuserDetails { get; set; }
    }
}
