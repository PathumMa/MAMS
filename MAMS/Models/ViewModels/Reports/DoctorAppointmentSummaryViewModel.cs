namespace MAMS.Models.ViewModels.Reports
{
    public class DoctorAppointmentSummaryViewModel
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int AppointmentNumber { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string Available_Day { get; set; }
        public TimeSpan? TimeSlot { get; set; }
        public int Status { get; set; }
        public DateTime? IssuedDate { get; set; }
    }
}
