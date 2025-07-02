namespace MAMS.Models.ViewModels
{
    public class AppointmentsViewModel
    {
        // From Appointments
        public int Id { get; set; }
        public string User_PersonalId { get; set; }
        public int Doctor_Id { get; set; }
        public int Availability_Id { get; set; }
        public DateTime Appointment_Date { get; set; }
        public int Appoinment_number { get; set; }
        public string Status { get; set; }

        // From PatientDetails
        public string PatientName { get; set; }
        public string PatientTitle { get; set; }
        public string PersonalId { get; set; }
        public string PersonalIdType { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }

        // From Transactions
        public decimal? Doctor_fee { get; set; }
        public decimal? Hospital_fee { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }

        // Doctor Info
        public string DoctorName { get; set; }
        public string DoctorSpecialization { get; set; }
    }
}
