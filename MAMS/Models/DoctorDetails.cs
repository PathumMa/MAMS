namespace MAMS.Models
{
    public class DoctorDetails
    {
        public int Id { get; set; }
        public int SuserDetailsId { get; set; }
        public string Speciality { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public SuserDetails SuserDetails { get; set; }
    }
}
