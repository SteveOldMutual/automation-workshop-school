using Automation_School_API.Commands.UserManagement;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using Automation_School_API_Tests.Fixtures;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_School_API_Tests
{
    public class UserManagementTests : TestFixture
    {
        [Fact]
        public async Task CanRetrieveUserList() 
        {
            //Arrange
            var user1 = new User()
            {
                SchoolId = "1",
                Username = "list1",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("list1"),
                Email = "SomeEmail@yes.com"
            };
            var user2 = new User()
            {
                SchoolId = "1",
                Username = "list2",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("list2"),
                Email = "SomeEmail@yes.com"
            };

            User[] users = new User[] { user1, user2 };

            await InsertAsync(users);

            List<UserDetailsDTO> foundUsers = await Mediator.Send(new GetUsersCommand());

            foundUsers.FirstOrDefault(x => x.Username.Equals(user1.Username)).Should().NotBeNull();
            foundUsers.FirstOrDefault(x => x.Username.Equals(user2.Username)).Should().NotBeNull();
        }
    }
}
