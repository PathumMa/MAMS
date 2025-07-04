namespace MAMS.Models.ViewModels
{
    public class LabTestViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public List<int> SelectedCategoryIds { get; set; } = new();
        public List<LabTestCategoryViewModel> AvailableCategories { get; set; } = new();
    }
}
