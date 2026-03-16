using Automation_School_API.Commands.Authentication;
using Automation_School_API.Commands.UserManagement;
using Automation_School_API.Domain.Authentication;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Automation_School_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    
    public class UserManagementController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IMediator _mediator;

        public UserManagementController(IMediator mediator, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _mediator.Send(new GetUsersCommand());
           
            return Ok(response);
        }

        [HttpPost("getSchoolId")]
        public async Task<IActionResult> GetSchoolId([FromBody] GetSchoolIDDTO schoolDTO){
            var response = await _mediator.Send(new GetSchoolCommand(schoolDTO.Token));
            return Ok(response);
        }
    }
        
}
