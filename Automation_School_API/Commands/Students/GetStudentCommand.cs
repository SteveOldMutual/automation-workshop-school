using Automation_School_API.Context;
using Automation_School_API.Domain.Authentication;
using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Authentication
{
    public record GetStudentCommand(string schoolId, string id) : IRequest<Student>;
    public class GetStudentCommandHandler : IRequestHandler<GetStudentCommand, Student>
    {
        private readonly IStudentManagement _studentManagement;
        public GetStudentCommandHandler(IStudentManagement studentManagement)
        {
            _studentManagement = studentManagement;
        }

        public async Task<Student> Handle(GetStudentCommand request, CancellationToken cancellationToken) 
        {
            return await _studentManagement.GetStudent(request.schoolId, request.id);
        }
    }

    public record GetStudentsCommand(string schoolId) : IRequest<List<Student>>;
    public class GetStudentsCommandHandler : IRequestHandler<GetStudentsCommand, List<Student>>
    {
        private readonly IStudentManagement _studentManagement;
        public GetStudentsCommandHandler(IStudentManagement studentManagement)
        {
            _studentManagement = studentManagement;
        }

        public async Task<List<Student>> Handle(GetStudentsCommand request, CancellationToken cancellationToken)
        {
            return await _studentManagement.GetStudents(request.schoolId);
        }
    }
}
