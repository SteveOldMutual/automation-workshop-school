using Automation_School_API.Context;
using Automation_School_API.Domain.Authentication;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using MediatR;

namespace Automation_School_API.Commands.Authentication
{
    public record LoginCommand(LoginDTO dto) : IRequest<string>;
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IAuthService _authService;
        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken) 
        {
            return await _authService.AuthenticateUserAsync(request.dto);
        }
    }
}
