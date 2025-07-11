using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
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
        public LabCategory LabCategory { get; set; }
        public ActiveStatus IsActive { get; set; } = ActiveStatus.Active;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        public ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();
    }
}
