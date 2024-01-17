namespace MAMS.Models
{
    public class UserDetailsViewModel
    {
        public int SuserId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserTitle { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string? Middle_Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public DateTime Birth_Date { get; set; }
        public string Gender { get; set; }
        public string Blood_Type { get; set; }
        public string Personal_Id { get; set; }
        public string PersonalId_Type { get; set; }
        public string? AvailableDay { get; set; }
        public string? AvailableTime { get; set; }
        public string? Speciality { get; set; }
    }
}
