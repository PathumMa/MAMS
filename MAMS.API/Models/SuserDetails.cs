using System.ComponentModel.DataAnnotations;

namespace MAMS.API.Models
{
    public class SuserDetails
    {
        [Key]
        public int Id { get; set; }
        public int SuserId { get; set; }
        public string UserTitle { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string? Middle_Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        //public int Birth_Year { get; set; }
        public DateTime Birth_Date { get; set;}
        public string Gender { get; set; }
        public string Blood_Type { get; set; }
        public string Personal_Id { get; set; }
        public string PersonalId_Type { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Suser Suser { get; set; }
        public List<DoctorAvailableDetails> DoctorAvailableDetails { get; set; }
        public DoctorDetails DoctorDetails { get; set; }
    }
}
