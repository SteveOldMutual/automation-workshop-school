using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Students
{
    public record UpdateStudentCommand(string id, StudentDTO Student) : IRequest<Student>;
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Student>
    {
        private readonly IStudentManagement _studentManagement;
        public UpdateStudentCommandHandler(IStudentManagement studentManagement)
        {
            _studentManagement = studentManagement;
        }

        public async Task<Student> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            return await _studentManagement.UpdateStudent(request.id, request.Student);
        }
    }
}
