using Automation_School_API.Commands.Authentication;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using Automation_School_API_Tests.Fixtures;
using FluentAssertions;

namespace Automation_School_API_Tests
{
    public class AuthenticationTests : TestFixture
    {
        [Fact]
        public async Task UserCanAuthenticate()
        {
            LoginDTO loginDTO = new() { Username ="Test", Password =  "LolYes123" };

            //Arrange
            User user = new()
            {
                SchoolId = "1",
                Username = loginDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDTO.Password),
                Email = "SomeEmail@yes.com"
            };


            await InsertAsync(user);

            //Act

            var response = await Mediator.Send(new LoginCommand(loginDTO));

            //Assert
            //Token should be returned
             response.Should().NotBeNull();
        }

        [Fact]
        public async Task InvalidPasswordShouldNotAuth()
        {
            LoginDTO loginDTO = new() { Username ="InvalidPasswordTest", Password =  "LolYes123" };

            //Arrange
            User user = new()
            {
                Username = loginDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDTO.Password),
                Email = "SomeEmail@yes.com",
                SchoolId = "1"
            };


            await InsertAsync(user);

            loginDTO.Password = "incorrect";
            //Act

            var response = await Mediator.Send(new LoginCommand(loginDTO));

            //Assert
            //Token should not be returned
            response.Should().BeNull();
        }

        [Fact]
        public async Task InvalidUsernameShouldNotAuth()
        {
            LoginDTO loginDTO = new() { Username ="InvalidUsernameTest", Password =  "LolYes123" };

            //Arrange
            User user = new()
            {
                Username = loginDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDTO.Password),
                Email = "SomeEmail@yes.com",
                SchoolId = "1"
            };


            await InsertAsync(user);

            loginDTO.Username = "incorrect";
            //Act

            var response = await Mediator.Send(new LoginCommand(loginDTO));

            //Assert
            //Token should not be returned
            response.Should().BeNull();
        }

        [Fact]
        public async Task UserCanBeRegistered()
        {
            RegisterDTO registerDTO = new() {  Username ="RegisterTest", Password =  "LolYes123" , Email= "Another@email.com"};

            //Act

            var response = await Mediator.Send(new RegisterCommand(registerDTO));

            //Assert
            //Registration should return a token
            response.Should().NotBeNull();
            //User should be in the DB
            Context.Users.FirstOrDefault(x => x.Username == registerDTO.Username).Should().NotBeNull();
        }

        [Fact]
        public async Task UsernameCannotBeRegisteredTwice()
        {
            RegisterDTO registerDTO = new() { Username ="DuplicateRegistration", Password =  "LolYes123", Email= "Another@email.com" };
            User user = new()
            {
                SchoolId= "1",
                Username = registerDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                Email = registerDTO.Email
            };


            await InsertAsync(user);
            //Act

            var response = await Mediator.Send(new RegisterCommand(registerDTO));

            //Assert
            //Registration should return a token
            response.Should().Be("User already exists");
            //User should be in the DB
            Context.Users.Where(x => x.Username == registerDTO.Username).Count().Should().Be(1);
        }
    }


}
