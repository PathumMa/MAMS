using System.ComponentModel.DataAnnotations;
using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models
{
    public class LabResult
    {
        public int LabResultId { get; set; }
        public int LabTypeId { get; set; }
        public int PatientId { get; set; }

        public DateTime BookedDate { get; set; }
        public TimeSpan? TimeSlot { get; set; }
        public AppoinmentStatus Status { get; set; } // Pending, Completed, Cancelled

        public DateTime? PerformedDate { get; set; }
        public string? ResultValue { get; set; }
        public string? Comments { get; set; }
        public bool IsResultAvailable { get; set; }
        public decimal BookedPrice { get; set; }

        public string ReferenceNo { get; set; }

        public virtual PatientDetails Patient { get; set; }
        public virtual LabType LabType { get; set; }
    }
}
