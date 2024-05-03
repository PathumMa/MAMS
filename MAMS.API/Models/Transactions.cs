using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserDetailsId { get; set; }
        [Required]
        public int AppointmentId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime Created_Date { get; set; } = DateTime.Now;
        public string? Created_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string? Modified_By { get; set; }

        public UserDetails UserDetails { get; set; }
        public Appoinments appoinments { get; set; }
    }


}
