namespace MAMS.API.Models
{
    public class LabTestCategory
    {
        public int LabTestCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }

        public ICollection<LabTestLabTestCategory> LabTestLabTestCategories { get; set; } = new List<LabTestLabTestCategory>();
    }
}
