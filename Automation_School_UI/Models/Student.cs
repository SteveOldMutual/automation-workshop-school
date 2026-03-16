using System.ComponentModel.DataAnnotations;

namespace Automation_School_UI.Models
{
    public class Student : StudentDTO
    {
        public string Id { get; set; }

    }

    public class StudentDTO
    {
        public string SchoolId { get; set; }
        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }
        [Range(6, 19, ErrorMessage = "Age must be greater than 5 and less than 20.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Grade is required")]
        public string Grade { get; set; }
    }
}
