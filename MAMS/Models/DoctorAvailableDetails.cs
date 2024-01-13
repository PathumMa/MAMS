namespace MAMS.Models
{
    public class DoctorAvailableDetails
    {
        public int Id { get; set; }
        public int SuserDetailsId { get; set; }
        public string? AvailableDay { get; set; }
        public string? AvailableTime { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public SuserDetails SuserDetails { get; set; }
    }
}
