using static MAMS.Services.Enums;

namespace MAMS.Models.ViewModels
{
    public class LabResultUpdateViewModel
    {
        public int LabResultId { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? PerformedDate { get; set; }
        public string? ResultValue { get; set; }
        public string? Comments { get; set; }
        public AppoinmentStatus Status { get; set; }
    }
}

