using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MAMS.API.Models
{
    public class LabCategory
    {
        [Key]
        public int LabCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public DateTime? Created_Date { get; set; } = DateTime.Now;
        public string? Created_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string? Modified_By { get; set; }

        // Navigation
        public ICollection<LabType> LabTypes { get; set; } = new List<LabType>();
    }
}
