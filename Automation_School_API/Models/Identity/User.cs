namespace Automation_School_API.Models.Identity
{
    public class User
    {
        public string Id { get;set; } = Guid.NewGuid().ToString();
        public string Username { get;set; }
        public string PasswordHash { get;set; }
        public string Email { get;set; }
        public string SchoolId { get; set; }
    }
}
