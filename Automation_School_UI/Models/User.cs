using System.ComponentModel.DataAnnotations;

namespace Automation_School_UI.Models
{
    public class User 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
    }
}
