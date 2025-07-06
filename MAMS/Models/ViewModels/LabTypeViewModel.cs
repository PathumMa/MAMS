using static MAMS.Services.Enums;

namespace MAMS.Models.ViewModels
{
    public class LabTypeViewModel
    {
        public int LabTypeId { get; set; }

        public string LabName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public ActiveStatus IsActive { get; set; }

        public int LabCategoryId { get; set; }

        public string? CategoryName { get; set; } // Optional for display in table
    }
}
