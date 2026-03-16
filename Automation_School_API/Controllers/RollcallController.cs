using Automation_School_API.Commands.Authentication;
using Automation_School_API.Commands.Rollcall;
using Automation_School_API.Commands.Students;
using Automation_School_API.Commands.UserManagement;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using Automation_School_API.Models.Students;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Automation_School_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class RollcallController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMediator _mediator;

        public RollcallController(IMediator mediator, ILogger<StudentController> logger)
        { 
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetRollCallInfo([FromQuery] string schoolId, [FromQuery] DateTime dateTime)
        {
            var response = await _mediator.Send(new GetRollcallMetadataCommand(schoolId,dateTime));
           
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRollCallInfo([FromQuery] string schoolId, string id)
        {
            var response = await _mediator.Send(new GetStudentRollcallDataCommand(schoolId,id));

            return Ok(response);
        }


        [HttpGet("MarkPresent/{id}")]
        public async Task<IActionResult> Present([FromQuery] string schoolId,string id)
        {
            var response = await _mediator.Send(new SetPresentCommand(schoolId, id, true));
            switch (response) 
            {
                case ROLLCALLOUTCOME.SUCCESS:
                    return Ok(response.ToString());
                
                default: 
                    return BadRequest(response.ToString());
            }
        }

        [HttpGet("MarkAbsent/{id}")]
        public async Task<IActionResult> Absent([FromQuery] string schoolId,string id)
        {
            var response = await _mediator.Send(new SetPresentCommand(schoolId,id, false));
            switch (response)
            {
                case ROLLCALLOUTCOME.SUCCESS:
                    return Ok(response.ToString());

                default:
                    return BadRequest(response.ToString());
            }
        }

        [HttpGet("Dismiss/{id}")]
        public async Task<IActionResult> Dismiss(string id)
        {
            var response = await _mediator.Send(new DismissStudentCommand(id));
            switch (response)
            {
                case ROLLCALLOUTCOME.SUCCESS:
                    return Ok(response);

                default:
                    return BadRequest(response);
            }
        }

        [HttpGet("UndoDismiss/{id}")]
        public async Task<IActionResult> UndoDismiss(string id)
        {
            var response = await _mediator.Send(new UndoDismissStudentCommand(id));
            switch (response)
            {
                case ROLLCALLOUTCOME.SUCCESS:
                    return Ok(response);

                default:
                    return BadRequest(response);
            }
        }


    }
        
}
