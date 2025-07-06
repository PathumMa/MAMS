using System.ComponentModel.DataAnnotations;

namespace MAMS.API.Models
{
    public class LabResult
    {
        [Key]
        public int LabResultId { get; set; }

        public int LabTypeId { get; set; }
        public LabType LabType { get; set; }

        public int PatientId { get; set; } // FK to your Patient table

        public DateTime BookedDate { get; set; }
        public DateTime? PerformedDate { get; set; }

        public string? ResultValue { get; set; }
        public string? Comments { get; set; }

        public bool IsResultAvailable { get; set; } = false;
        public decimal BookedPrice { get; set; }

    }
}
