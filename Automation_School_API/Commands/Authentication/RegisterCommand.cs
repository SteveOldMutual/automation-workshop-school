using Automation_School_API.Context;
using Automation_School_API.Domain.Authentication;
using Automation_School_API.Models.DTOs;
using Automation_School_API.Models.Identity;
using MediatR;

namespace Automation_School_API.Commands.Authentication
{
    public record RegisterCommand(RegisterDTO dto) : IRequest<string>;
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IAuthService _authService;
        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken) 
        {
            return await _authService.RegisterUserAsync(request.dto);
        }
    }
}
