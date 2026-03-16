using Automation_School_API.Context;
using Automation_School_API.Domain.Authentication;
using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Authentication
{
    public record BulkCreateStudentCommand(StudentDTO[] dto) : IRequest<List<Student>>;
    public class BulkCreateStudentCommandHandler : IRequestHandler<BulkCreateStudentCommand, List<Student>>
    {
        private readonly IStudentManagement _studentManagement;
        public BulkCreateStudentCommandHandler(IStudentManagement studentManagement)
        {
            _studentManagement = studentManagement;
        }

        public async Task<List<Student>> Handle(BulkCreateStudentCommand request, CancellationToken cancellationToken) 
        {
            return await _studentManagement.BulkCreateStudents(request.dto);
        }
    }
}
