namespace MAMS.API.Tools
{
    public class Enums
    {
        public enum ActiveStatus
        {
            Inactive = 0,
            Active = 1
        }

        public enum AuthStatus
        {
            Pending = 0,
            Authorized = 1,
            Rejected = 2
        }

        public enum AppoinmentStatus
        {
            Scheduled = 1,
            Completed = 2,
            canceled = 3,
            Postponed = 4
        }

        public enum UserRole
        {
            Admin = 10,
            Patient = 20,
            Doctor = 30
        }

        public enum GenderType
        {
            Male = 1,
            Female = 2
        }

        public enum UserTitleType
        {
            Mr = 1,
            Mrs = 2,
            Miss = 3,
            Dr = 4,
            Proff = 5,
            Rev = 6,
            Other = 7 // You can add more specific titles as needed
        }

        public enum PersonalIdType
        {
            Nic = 1,
            Passport = 2,
            DLicence = 3
        }

        public enum Province
        {
            Central,
            Eastern,
            NorthCentral,
            Northern,
            NorthWestern,
            Sabaragamuwa,
            Southern,
            Uva,
            Western
        }

        public enum District
        {
            Ampara,
            Anuradhapura,
            Badulla,
            Batticaloa,
            Colombo,
            Galle,
            Gampaha,
            Hambantota,
            Jaffna,
            Kalutara,
            Kandy,
            Kegalle,
            Kilinochchi,
            Kurunegala,
            Mannar,
            Matale,
            Matara,
            Monaragala,
            Mullaitivu,
            NuwaraEliya,
            Polonnaruwa,
            Puttalam,
            Ratnapura,
            Trincomalee,
            Vavuniya
        }

        public enum BloodGroupType
        {
            A_Positive,
            A_Negative,
            B_Positive,
            B_Negative,
            AB_Positive,
            AB_Negative,
            O_Positive,
            O_Negative
        }

        public enum DoctorSpecialty
        {
            General_Practitioner,
            Cardiologist,
            Dermatologist,
            Endocrinologist,
            Gastroenterologist,
            Neurologist,
            OrthopedicSurgeon,
            Ophthalmologist,
            Otolaryngologist,
            Pediatrician,
            Psychiatrist,
            Pulmonologist,
            Rheumatologist,
            Urologist,
            Oncologist
        }

    }
}
