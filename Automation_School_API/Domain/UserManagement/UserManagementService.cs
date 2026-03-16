using Automation_School_API.Context;
using Automation_School_API.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Automation_School_API.Domain.UserManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IConfiguration _configuration;
        private readonly SchoolDBContext _schoolDBContext;
        public UserManagementService(IConfiguration config, SchoolDBContext context)
        {
            _configuration = config;
            _schoolDBContext = context;
        }
        public async Task<User> GetUser(string id)
        {
            var user = await _schoolDBContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) {
                return null;
            }
            return user;
        }

        public async Task<List<UserDetailsDTO>> GetUsers()
        {
            var users =  _schoolDBContext.Users.ToList();
            return users.Select(x => new UserDetailsDTO { Username = x.Username, Id =  x.Id, Email = x.Email, SchoolId=x.SchoolId}).ToList();
        }

        public async Task RemoveUser(User user)
        {
            var foundUser = await _schoolDBContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (foundUser == null)
            {
                throw new Exception("User does not exist");
            }

            _schoolDBContext.Users.Remove(foundUser);
            await _schoolDBContext.SaveChangesAsync();

        }

        public async Task<User> UpdateUser(User user)
        {
            var foundUser = await _schoolDBContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (foundUser == null)
            {
                throw new Exception("User does not exist");
            }

            foundUser = user;
            await _schoolDBContext.SaveChangesAsync();

            return foundUser;
        }

       
    }
}
