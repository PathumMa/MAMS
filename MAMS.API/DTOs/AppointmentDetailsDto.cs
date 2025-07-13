namespace MAMS.API.DTOs
{
    public class AppointmentDetailsDto
    {
        public int Id { get; set; }
        public string User_PersonalId { get; set; }
        public int Doctor_Id { get; set; }
        public int Availability_Id { get; set; }
        public DateTime Appointment_Date { get; set; }
        public int Appoinment_number { get; set; }
        public string Status { get; set; }

        public string PatientName { get; set; }
        public string PatientTitle { get; set; }
        public string PersonalId { get; set; }
        public string PersonalIdType { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string DoctorName { get; set; }
        public string DoctorSpecialization { get; set; }

        public List<TransactionDto> Transactions { get; set; }
    }
}
