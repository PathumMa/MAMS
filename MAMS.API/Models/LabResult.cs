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
        public DateTime? Created_Date { get; set; } = DateTime.Now;
        public string? Created_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string? Modified_By { get; set; }

        public virtual PatientDetails Patient { get; set; }
        public virtual LabType LabType { get; set; }
    }
}
