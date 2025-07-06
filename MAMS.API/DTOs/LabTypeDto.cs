using System.ComponentModel.DataAnnotations.Schema;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.DTOs
{
    public class LabTypeDto
    {
        public string LabName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ActiveStatus IsActive { get; set; }
        public int LabCategoryId { get; set; }
    }
}
