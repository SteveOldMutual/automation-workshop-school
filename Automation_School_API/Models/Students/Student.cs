namespace Automation_School_API.Models.Students
{
    public class Student : StudentDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public StudentDTO ToStudentDTO()
        {
            return new StudentDTO
            {
                SchoolId = this.SchoolId,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Age = this.Age,
                Grade = this.Grade
            };
        }
    }

    public class StudentDTO
    {
        public string SchoolId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Grade { get; set; }
    }
}
