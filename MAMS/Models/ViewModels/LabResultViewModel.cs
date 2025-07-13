using static MAMS.Services.Enums;

namespace MAMS.Models.ViewModels
{
    public class LabResultViewModel
    {
        public int LabResultId { get; set; }
        public string ReferenceNo { get; set; }
        public int LabTypeId { get; set; }
        public string LabTypeName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime BookedDate { get; set; }
        public TimeSpan? TimeSlot { get; set; }
        public DateTime? PerformedDate { get; set; }
        public string? ResultValue { get; set; }
        public string? Comments { get; set; }
        public AppoinmentStatus Status { get; set; }
        public decimal BookedPrice { get; set; }
        public DateTime? Modified_Date { get; set; }
    }
}
