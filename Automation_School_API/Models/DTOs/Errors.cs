namespace Automation_School_API.Models.DTOs
{
    public class Errors
    {
        public List<string> _errors { get; set; } = new List<string>();
        public void AddError(string error) 
        {
            _errors.Add(error);
        }
    }
}
