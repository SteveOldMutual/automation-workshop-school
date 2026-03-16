using Automation_School_API.Context;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Automation_School_API.Domain.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly SchoolDBContext _schoolDBContext;
        public AuthService(IConfiguration config, SchoolDBContext context) 
        { 
            _configuration = config;
            _schoolDBContext = context;
        }

        public async Task<string> AuthenticateUserAsync(LoginDTO loginDto)
        {
           var user = await _schoolDBContext.Users.SingleOrDefaultAsync(x => x.Username == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) 
            {
                return null;
            }
            return GenerateJwtToken(user);
        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Authenticated","true"),
                new Claim("Role","Administrator"),
                new Claim("SchoolId",user.SchoolId.ToString())
            };

            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> RegisterUserAsync(RegisterDTO reg)
        {
            var user = _schoolDBContext.Users.Where(x => x.Username == reg.Username).FirstOrDefault();
            if (user == null) 
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = reg.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(reg.Password),
                    Email = reg.Email,
                    SchoolId = Guid.NewGuid().ToString()
                };

                await _schoolDBContext.Users.AddAsync(user);
                await _schoolDBContext.SaveChangesAsync();

                return GenerateJwtToken(user);
            }
            return "User already exists";
        }
    }
}
