using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Rollcall
{
    public record SetPresentCommand(string schoolId, string id, bool present): IRequest<ROLLCALLOUTCOME>;
    public class SetPresentCommandHandler : IRequestHandler<SetPresentCommand, ROLLCALLOUTCOME> 
    {
        private readonly IRollCallService _rollCallService;
        public SetPresentCommandHandler(IRollCallService rollCallService) 
        {
            _rollCallService = rollCallService;
        }

        public async Task<ROLLCALLOUTCOME> Handle(SetPresentCommand request, CancellationToken cancellationToken) 
        {
            if (request.present) 
            {
               return await _rollCallService.MarkStudentPresent(request.schoolId, request.id);
            }

            return await _rollCallService.MarkStudentAbsent(request.schoolId,request.id);
        }
    }
    
}
