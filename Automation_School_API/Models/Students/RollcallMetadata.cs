namespace Automation_School_API.Models.Students
{
    public class RollcallMetadata
    {
        public DateTime RollCallDate { get; set; }
        public List<Rollcall> Rollcalls { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalDismissed { get; set; }
    }
}
