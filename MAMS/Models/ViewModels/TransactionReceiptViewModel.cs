using static MAMS.Services.Enums;

namespace MAMS.Models.ViewModels
{
    public class TransactionReceiptViewModel
    {
        public string ReferenceNo { get; set; }
        public string PatientName { get; set; }
        public string LabTestName { get; set; }
        public AppoinmentStatus Status { get; set; }
        public string BookedDate { get; set; }
        public string TimeSlot { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public string IssuedDate { get; set; }
    }
}
