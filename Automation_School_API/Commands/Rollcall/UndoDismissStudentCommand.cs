using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Rollcall
{
    public record UndoDismissStudentCommand(string id): IRequest<ROLLCALLOUTCOME>;
    public class UndoDismissStudentCommandHandler : IRequestHandler<UndoDismissStudentCommand, ROLLCALLOUTCOME> 
    {
        private readonly IRollCallService _rollCallService;
        public UndoDismissStudentCommandHandler(IRollCallService rollCallService) 
        {
            _rollCallService = rollCallService;
        }

        public async Task<ROLLCALLOUTCOME> Handle(UndoDismissStudentCommand request, CancellationToken cancellationToken) 
        {
            return await _rollCallService.UndoDismiss(request.id);
        }
    }
    
}
