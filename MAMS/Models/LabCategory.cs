namespace MAMS.Models
{
    public class LabCategory
    {
        public int LabCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }

        // Navigation
        public LabType LabTypes { get; set; }
    }
}
