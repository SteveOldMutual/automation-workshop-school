using Automation_School_API.Context;
using Automation_School_API.Domain.Authentication;
using Automation_School_API.Domain.UserManagement;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using MediatR;

namespace Automation_School_API.Commands.UserManagement
{
    public record GetUsersCommand() : IRequest<List<UserDetailsDTO>>;
    public class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, List<UserDetailsDTO>>
    {
        private readonly IUserManagementService _managementService;
        public GetUsersCommandHandler( IUserManagementService userManagementService)
        {
            _managementService = userManagementService;
        }

        public async Task<List<UserDetailsDTO>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
        {
            return await _managementService.GetUsers();
        }
    }
}
