namespace MAMS.API.Models
{
    public class LabTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<LabTestResult> LabTestResults { get; set; } = new List<LabTestResult>();
        public ICollection<LabTestLabTestCategory> LabTestLabTestCategories { get; set; } = new List<LabTestLabTestCategory>();
    }
}
