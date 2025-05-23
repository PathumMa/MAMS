using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.DTOs
{
    public class BookingDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserTitle { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonalId_Type { get; set; }
        public string Personal_Id { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Availability_Id { get; set; }
        public int Doctor_Id { get; set; }
        public string Available_Day { get; set; }
        public DateTime Appointment_Date { get; set; }
        public int Appoinment_number { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal Doctor_fee { get; set; }
        public decimal Hospital_fee { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal Amount { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        public ActiveStatus Status { get; set; } = ActiveStatus.Active;


    }
}
