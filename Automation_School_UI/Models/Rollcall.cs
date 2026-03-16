using System.ComponentModel.DataAnnotations;

namespace Automation_School_UI.Models
{
    public class Rollcall
    {

        public string RollCallId { get; set; } = Guid.NewGuid().ToString();
        public string studentId { get; set; }
        public Student Student { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public bool Dismissed { get; set; }
        public DateTime DismissedTimestamp { get; set; }
    }
}
