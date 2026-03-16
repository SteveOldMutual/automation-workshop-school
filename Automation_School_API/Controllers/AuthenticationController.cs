using Automation_School_API.Commands.Authentication;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Automation_School_API.Controllers
{
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
           
            var response = await _mediator.Send(new LoginCommand(user));
            if (response == null)
            {
                return Unauthorized();
            }
            return Ok(new { Token = response });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO user)
        {
            if (user.Username.Length > 50)
            {
                return BadRequest("Username exceeds maximum length of 50.");
            }
            if (user.Password.Length > 50)
            {
                return BadRequest("Password exceeds maximum length of 50.");
            }

            var response = await _mediator.Send(new RegisterCommand(user));
            if (response == null)
            {
                return BadRequest(new { Message = "Registration failed" });
            }
            if (response == "User already exists") 
            {
                return BadRequest(new { Message = "Username already registered" });
            }
            return Ok(new { Token = response });
        }
    }
}
