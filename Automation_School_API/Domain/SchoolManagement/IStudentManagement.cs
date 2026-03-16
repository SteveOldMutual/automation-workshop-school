using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Students;

namespace Automation_School_API.Domain.SchoolManagement
{
    public interface IStudentManagement
    {
        public Task<Student> CreateStudent(StudentDTO dto);
        public Task<List<Student>> BulkCreateStudents(StudentDTO[] studentDTOs);
        public Task<Student> UpdateStudent(string id, StudentDTO updatedInfo);
        public Task<Student> GetStudent(string schoolId, string id);
        public Task<List<Student>> GetStudents(string schoolId);
        public Task<bool> DeleteStudent(string id);
        public Task<SchoolIDDTO> GetSchoolId(string token);



    }
}
