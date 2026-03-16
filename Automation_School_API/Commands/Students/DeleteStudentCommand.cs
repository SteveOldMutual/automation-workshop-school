using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Students
{
    public record DeleteStudentCommand(string id) : IRequest<bool>;
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IStudentManagement _studentManagement;
        public DeleteStudentCommandHandler(IStudentManagement studentManagement)
        {
            _studentManagement = studentManagement;
        }

        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            return await _studentManagement.DeleteStudent(request.id);
        }
    }
}
