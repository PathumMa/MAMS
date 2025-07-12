using static MAMS.API.Tools.Enums;

namespace MAMS.API.DTOs
{
    public class LabBookingResponseDto
    {
        public string ReferenceNo { get; set; }
        public string Time { get; set; }
        public string BookedDate { get; set; }
        public AppoinmentStatus Status { get; set; }
        public string LabName { get; set; }
        public decimal Price { get; set; }
        public string Patient { get; set; }
    }
}
