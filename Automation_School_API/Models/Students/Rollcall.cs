using System.ComponentModel.DataAnnotations;

namespace Automation_School_API.Models.Students
{
    public class Rollcall
    {

        public int RollCallId { get; set; } 
        public string studentId { get; set; }
        public string SchoolId {get;set;}
        public Student Student { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public bool Dismissed { get; set; }
        public DateTime DismissedTimestamp { get; set; }
    }

    public enum ROLLCALLOUTCOME { SUCCESS, ALREADYPRESENT, ALREADYABSENT, ALREADYDISMISSED, NOTPRESENT, ERROR, NOTDISMISSED }
}
