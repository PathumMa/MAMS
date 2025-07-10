using static MAMS.API.Tools.Enums;

namespace MAMS.API.DTOs
{
    public class LabBookingDto
    {
        public string PersonalId { get; set; }        // NIC or other
        public string PersonalIdType { get; set; }    // NIC, Passport, etc.
        public string UserTitle { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }

        public int LabTypeId { get; set; }
        public DateTime BookedDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash; // Default to Cash
    }
}
