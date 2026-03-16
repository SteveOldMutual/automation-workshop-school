using Automation_School_API.Context;
using Automation_School_API.Domain.Authentication;
using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Authentication
{
    public record CreateStudentCommand(StudentDTO dto) : IRequest<Student>;
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Student>
    {
        private readonly IStudentManagement _studentManagement;
        public CreateStudentCommandHandler(IStudentManagement studentManagement)
        {
            _studentManagement = studentManagement;
        }

        public async Task<Student> Handle(CreateStudentCommand request, CancellationToken cancellationToken) 
        {
            return await _studentManagement.CreateStudent(request.dto);
        }
    }
}
