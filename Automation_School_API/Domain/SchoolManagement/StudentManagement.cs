using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Automation_School_API.Context;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Automation_School_API.Domain.SchoolManagement
{
    public class StudentManagement : IStudentManagement
    {
        private readonly IConfiguration _configuration;
        private readonly SchoolDBContext _schoolDBContext;
        public StudentManagement(IConfiguration config, SchoolDBContext context)
        {
            _configuration = config;
            _schoolDBContext = context;
        }

        public async Task<List<Student>> BulkCreateStudents(StudentDTO[] studentDTOs)
        {
            List<Student> students = new List<Student>();
            foreach(var student in studentDTOs) 
            {
                var created = await CreateStudent(student);
                students.Add(created);
            }
            return students;
        }

        public async Task<Student> CreateStudent(StudentDTO dto)
        {
            var student = new Student()
            {
                SchoolId = dto.SchoolId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age,
                Grade = dto.Grade,
            };
            await _schoolDBContext.Students.AddAsync(student);
            await _schoolDBContext.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudent(string id)
        {
            var student = await _schoolDBContext.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (student != null)
            {
                try
                {
                    _schoolDBContext.Students.Remove(student);
                    await _schoolDBContext.SaveChangesAsync();
                    return true;
                }
                catch
                {

                }
            }
            return false;
        }

        public Task<SchoolIDDTO> GetSchoolId(string token)
        {
            // Parse the JSON to get the token
            var handler = new JwtSecurityTokenHandler();
            //this can check lifetime
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "YourIssuer",
                ValidAudience = "YourAudience",
                IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("YourSuperSecretKeyThatIsAtLeast32CharactersLong")
                        )
            };

            var principal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            if (principal == null && validatedToken is JwtSecurityToken jwtToken)
            {
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                principal = new ClaimsPrincipal(identity);
            }

            //var jwtToken = handler.ReadJwtToken(token);

            var claims = principal.Claims;
            var schoolId = claims.FirstOrDefault(x => x.Type == "SchoolId").Value;
            var dto = new SchoolIDDTO(){SchoolId = schoolId};
            return Task.FromResult(dto);
        }

        public async Task<Student> GetStudent(string schoolId, string id)
        {
            var student = await _schoolDBContext.Students.FirstOrDefaultAsync(x => x.Id == id && x.SchoolId == schoolId);
            if (student != null) return student;

            return null;
        }

        public async Task<List<Student>> GetStudents(string schoolId)
        {
            return await _schoolDBContext.Students.Where(x => x.SchoolId == schoolId).ToListAsync();
        }

        public async Task<Student> UpdateStudent(string id, StudentDTO updatedInfo)
        {
            var student = await _schoolDBContext.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (student != null)
            {
                if (updatedInfo.FirstName != null) student.FirstName = updatedInfo.FirstName;
                if (updatedInfo.LastName != null) student.LastName = updatedInfo.LastName;
                if (updatedInfo.Age != 0) student.Age = updatedInfo.Age;
                if (updatedInfo.Grade != null) student.Grade = updatedInfo.Grade;

                await _schoolDBContext.SaveChangesAsync();
                return student;
            }
            return null;
        }
    }
}
