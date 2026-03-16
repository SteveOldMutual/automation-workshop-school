using Automation_School_API.Models.Identity;

namespace Automation_School_API.Domain.UserManagement
{
    public interface IUserManagementService
    {
        public Task<User> GetUser(string id);
        public Task<List<UserDetailsDTO>> GetUsers();
        public Task<User> UpdateUser(User user);
        public Task RemoveUser(User user);


    }
}
