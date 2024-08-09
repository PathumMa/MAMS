using static MAMS.API.Tools.Enums;

namespace MAMS.API.Models.Views
{
    public class Doctors
    {
        public int SuserId { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Specialization { get; set; }
        public string Available_Day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public ActiveStatus ActiveStatus { get; set; }

    }
}
