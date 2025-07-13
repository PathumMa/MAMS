namespace MAMS.Models.ViewModels
{
    public class LabCategoryDetailsViewModel
    {
        public int LabCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }

        public List<LabTypeSummaryViewModel> Labs { get; set; } = new List<LabTypeSummaryViewModel>();
    }

    public class LabTypeSummaryViewModel
    {
        public int LabTypeId { get; set; }
        public string LabName { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
