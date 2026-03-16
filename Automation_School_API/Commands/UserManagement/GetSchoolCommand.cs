using Automation_School_API.Context;
using Automation_School_API.Domain.Authentication;
using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Domain.UserManagement;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using MediatR;

namespace Automation_School_API.Commands.UserManagement
{
    public record GetSchoolCommand(string token) : IRequest<SchoolIDDTO>;
    public class GetSchoolCommandHandler : IRequestHandler<GetSchoolCommand, SchoolIDDTO>
    {
        private readonly IStudentManagement _managementService;
        public GetSchoolCommandHandler( IStudentManagement studentManagement)
        {
            _managementService = studentManagement;
        }

        public async Task<SchoolIDDTO> Handle(GetSchoolCommand request, CancellationToken cancellationToken)
        {
            return await _managementService.GetSchoolId(request.token);
        }
    }
}
