using Automation_School_UI.Models;

namespace Automation_School_UI.Services
{
    public interface IUserManagementService
    {
        public Task<List<User>> GetUsers();
    }
}
