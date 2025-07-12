using MAMS.Services;
using MAMS.Validators;
using static MAMS.Services.Enums;

namespace MAMS.Models.ViewModels
{
    public class LabBookingViewModel
    {
        [SriLankanNIC(ErrorMessage = "Please enter a valid NIC (e.g., 931234567V or 200012345678). or Other")]
        public string PersonalId { get; set; }        // NIC or other
        public string PersonalIdType { get; set; }    // NIC, Passport, etc.
        public string UserTitle { get; set; }
        public string Name { get; set; }
        [SriLankanPhone]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public AppoinmentStatus Status { get; set; }
        public int LabTypeId { get; set; }
        public DateTime BookedDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash; // Default to Cash
    }
}
