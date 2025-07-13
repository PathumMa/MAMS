using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        public int? Appointment_Id { get; set; }

        [Required]
        public int PatientDetails_Id { get; set; }

        public BookingType BookingType { get; set; } = BookingType.Doctor;

        public decimal? Doctor_fee { get; set; }

        public int? LabResultId { get; set; }

        public decimal? Hospital_fee { get; set; }

        public decimal Discount { get; set; } = 0;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public DateTime Created_Date { get; set; } = DateTime.Now;

        public string? Created_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        public string? Modified_By { get; set; }

        [ForeignKey("Appointment_Id")]
        public virtual Appointments Appointments { get; set; }

        [ForeignKey("PatientDetails_Id")]
        public virtual PatientDetails PatientDetails { get; set; }

        [ForeignKey("LabResultId")]
        public virtual LabResult? LabResult { get; set; }
    }

}
