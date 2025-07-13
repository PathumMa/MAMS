using static MAMS.API.Tools.Enums;

namespace MAMS.API.DTOs
{
    public class LabResultUpdateDto
    {
        public int LabResultId { get; set; }
        public DateTime? PerformedDate { get; set; }
        public string? ResultValue { get; set; }
        public string? Comments { get; set; }
        public AppoinmentStatus Status { get; set; }
    }
}
