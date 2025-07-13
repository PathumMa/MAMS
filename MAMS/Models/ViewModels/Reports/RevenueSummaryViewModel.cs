namespace MAMS.Models.ViewModels.Reports
{
    public class RevenueSummaryViewModel
    {
        public DateTime RevenueDate { get; set; }

        public int DoctorAppointments { get; set; }
        public decimal DoctorRevenue { get; set; }

        public int LabBookings { get; set; }
        public decimal LabRevenue { get; set; }

        public int TotalTransactions { get; set; }
        public decimal TotalRevenue { get; set; }

        public decimal CashRevenue { get; set; }
        public decimal CardRevenue { get; set; }
        public decimal OnlineRevenue { get; set; }
    }
}
