using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
{
    public class Specializations
    {
        [Key]
        public int Specializations_Id { get; set; }
        [Required]
        public string Specializations_Name { get; set; }
        public string? Description { get; set; }
        public ActiveStatus Record_Status { get; set; } = ActiveStatus.Active;
    }
}
