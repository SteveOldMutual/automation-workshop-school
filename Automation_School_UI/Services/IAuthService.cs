using Automation_School_UI.Models;

namespace Automation_School_UI.Services
{
    public interface IAuthService
    {
        public Task<string> Authenticate(LoginDTO dto);
        public Task<HttpResponseMessage> RegisterUser(RegisterDTO dto);

    }
}
