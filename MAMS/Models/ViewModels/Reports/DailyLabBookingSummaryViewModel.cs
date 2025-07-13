namespace MAMS.Models.ViewModels.Reports
{
    public class DailyLabBookingSummaryViewModel
    {
        public DateTime ReportDate { get; set; }
        public List<LabBookingSummaryViewModel> Bookings { get; set; } = new();
        public int TotalBookings => Bookings.Count;
        public decimal TotalRevenue => Bookings.Sum(x => x.TotalPaid.Value);
    }
}
