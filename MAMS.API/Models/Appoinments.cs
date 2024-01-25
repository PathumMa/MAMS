using System.ComponentModel.DataAnnotations;

namespace MAMS.API.Models
{
    public class Appoinments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int User_PersonalId { get; set; }
        [Required]
        public int Doctor_PersonalId { get; set; }
        public DateTime Appoinment_Date { get; set; }
        public int Status { get; set; } = 1;
    }
}
