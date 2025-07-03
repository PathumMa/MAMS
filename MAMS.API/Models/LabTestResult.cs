namespace MAMS.API.Models
{
    public class LabTestResult
    {
        public int LabTestResultId { get; set; }
        public int LabTestId { get; set; }
        public LabTest LabTest { get; set; }

        public int PatientId { get; set; } // FK to Patient table
        public string ResultValue { get; set; }
        public DateTime ResultDate { get; set; }
    }
}
