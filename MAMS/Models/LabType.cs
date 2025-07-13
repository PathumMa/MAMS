using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MAMS.Services.Enums;

namespace MAMS.Models
{
    public class LabType
    {
        [Key]
        public int LabTypeId { get; set; }
        [Required]
        public string LabName { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public int LabCategoryId { get; set; }
        public ActiveStatus IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public LabResult LabResults { get; set; }
        public LabCategory LabCategory { get; set; }
    }
}
