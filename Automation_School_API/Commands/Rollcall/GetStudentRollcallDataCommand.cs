using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Rollcall
{
    public record GetStudentRollcallDataCommand(string schoolId,string id): IRequest<RollcallMetadata>;
    public class GetStudentRollcallDataCommandHandler : IRequestHandler<GetStudentRollcallDataCommand, RollcallMetadata> 
    {
        private readonly IRollCallService _rollCallService;
        public GetStudentRollcallDataCommandHandler(IRollCallService rollCallService) 
        {
            _rollCallService = rollCallService;
        }

        public async Task<RollcallMetadata> Handle(GetStudentRollcallDataCommand request, CancellationToken cancellationToken) 
        {
            return await _rollCallService.GetRollCallData(request.schoolId, request.id);
        }
    }
    
}
