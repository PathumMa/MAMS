using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Appointment_Id { get; set; }
        [Required]
        public decimal Doctor_fee { get; set; }
        [Required]
        public decimal Hospital_fee { get; set; }
        [Required]
        public decimal Discount { get; set; } = 0;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime Created_Date { get; set; } = DateTime.Now;
        public string? Created_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string? Modified_By { get; set; }

        
        public Appointments Appointments { get; set; }
    }


}
