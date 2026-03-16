using Automation_School_UI.Models;
using System.Reflection.Metadata;

namespace Automation_School_UI.Services
{
    public interface IStudentService
    {
        public Task<Student> CreateStudent(StudentDTO dto);
        public Task<Student> UpdateStudent(string id, StudentDTO updatedInfo);
        public Task<Student> GetStudent(string id);
        public Task<List<Student>> GetStudents();
        public Task<bool> DeleteStudent(string id);
    }
}
