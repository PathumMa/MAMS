namespace MAMS.Models.ViewModels.Reports
{
    public class LabBookingSummaryViewModel
    {
        public string ReferenceNo { get; set; }
        public DateTime BookedDate { get; set; }
        public string? TimeSlot { get; set; }
        public string? PatientName { get; set; }
        public string? LabName { get; set; }
        public decimal? TotalPaid { get; set; }
        public int? PaymentMethod { get; set; }
        public DateTime? IssuedDate { get; set; }
    }
}
