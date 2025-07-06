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

        // Navigation
        public ICollection<LabType> LabTypes { get; set; } = new List<LabType>();
    }
}
