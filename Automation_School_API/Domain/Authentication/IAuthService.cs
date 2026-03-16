using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;

namespace Automation_School_API.Domain.Authentication
{
    public interface IAuthService
    {
        public string GenerateJwtToken(User user);
        public  Task<string> RegisterUserAsync(RegisterDTO user);
        public Task<string> AuthenticateUserAsync(LoginDTO user);
    }
}
