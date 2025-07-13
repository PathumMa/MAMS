using static MAMS.Services.Enums;

namespace MAMS.Models.ViewModels
{
    public class LabCategoryViewModel
    {
        public int LabCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public ActiveStatus IsActive { get; set; }

        public int LabCount { get; set; }
    }
}
