using System.ComponentModel.DataAnnotations;

namespace MAMS.Models
{
    public class Specializations
    {
        [Key]
        public int Specializations_Id { get; set; }
        [Required]
        public string Specializations_Name { get; set; }
        public int Record_Status { get; set; } = 1;
    }
}
