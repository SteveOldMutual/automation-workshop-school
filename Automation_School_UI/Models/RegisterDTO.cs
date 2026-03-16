using System.ComponentModel.DataAnnotations;

namespace Automation_School_UI.Models
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }
    }
}
