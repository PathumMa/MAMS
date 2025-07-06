namespace MAMS.Models.ViewModels
{
    public class LabResultViewModel
    {
        public int LabResultId { get; set; }

        public int LabTypeId { get; set; }

        public string LabName { get; set; }

        public decimal BookedPrice { get; set; }

        public int PatientId { get; set; }

        public DateTime BookedDate { get; set; }

        public DateTime? PerformedDate { get; set; }

        public string? ResultValue { get; set; }

        public string? Comments { get; set; }

        public bool IsResultAvailable { get; set; }
    }
}
