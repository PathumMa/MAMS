namespace MAMS.Models.ViewModels
{
    public class LabBookingResponseViewModel
    {
        public string ReferenceNo { get; set; }
        public string Time { get; set; }
        public string BookedDate { get; set; }
        public string LabName { get; set; }
        public decimal Price { get; set; }
        public string Patient { get; set; }
    }
}
