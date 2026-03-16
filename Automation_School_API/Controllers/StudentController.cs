using Automation_School_API.Commands.Authentication;
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
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator, ILogger<StudentController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetStudents([FromQuery] string schoolId)
        {
            var response = await _mediator.Send(new GetStudentsCommand(schoolId));

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromQuery] string schoolId, string id)
        {
            var response = await _mediator.Send(new GetStudentCommand(schoolId, id));

            return Ok(response);
        }

       

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] StudentDTO dto)
        {
            var errors = new Errors();
            if (dto.SchoolId == null || dto.SchoolId  == "")
            {
                errors.AddError("Missing required field - SchoolId");
            }
            if (dto.FirstName.Length > 50) 
            {
                errors.AddError("FirstName exceeds maximum length of 50.");
            }
            if (dto.LastName.Length > 50)
            {
                errors.AddError("FirstName exceeds maximum length of 50.");
            }

            if (errors._errors.Count > 0) 
            {
                return BadRequest(errors);
            }
            
            var response = await _mediator.Send(new CreateStudentCommand(dto));

            return Ok(response);
        }

        [HttpPost("bulkCreate")]
        public async Task<IActionResult> BulkCreate([FromBody] StudentDTO[] dto)
        {
            var isNull = dto.Any(x => x.SchoolId == null);
            var isEmpty = dto.Any(x => x.SchoolId == "");   

            if(isNull || isEmpty)
            {
                return BadRequest("Missing required field - SchoolId");
            }

            var response = await _mediator.Send(new BulkCreateStudentCommand(dto));

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] StudentDTO dto)
        {
            var response = await _mediator.Send(new UpdateStudentCommand(id,dto));

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var response = await _mediator.Send(new DeleteStudentCommand(id));

            return Ok(response);
        }

    }
        
}
