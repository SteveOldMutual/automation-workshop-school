using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Rollcall
{
    public record DismissStudentCommand(string id): IRequest<ROLLCALLOUTCOME>;
    public class DismissStudentCommandHandler : IRequestHandler<DismissStudentCommand, ROLLCALLOUTCOME> 
    {
        private readonly IRollCallService _rollCallService;
        public DismissStudentCommandHandler(IRollCallService rollCallService) 
        {
            _rollCallService = rollCallService;
        }

        public async Task<ROLLCALLOUTCOME> Handle(DismissStudentCommand request, CancellationToken cancellationToken) 
        {
            return await _rollCallService.DismissStudent(request.id);
        }
    }
    
}
