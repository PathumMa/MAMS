namespace MAMS.API.Models
{
    public class LabTestLabTestCategory
    {
        public int LabTestId { get; set; }
        public LabTest LabTest { get; set; }

        public int LabTestCategoryId { get; set; }
        public LabTestCategory LabTestCategory { get; set; }
    }
}
