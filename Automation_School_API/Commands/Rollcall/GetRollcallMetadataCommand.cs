using Automation_School_API.Domain.SchoolManagement;
using Automation_School_API.Models.Students;
using MediatR;

namespace Automation_School_API.Commands.Rollcall
{
    public record GetRollcallMetadataCommand(string schoolId, DateTime date): IRequest<RollcallMetadata>;
    public class GetRollcallMetadataCommandHandler : IRequestHandler<GetRollcallMetadataCommand, RollcallMetadata> 
    {
        private readonly IRollCallService _rollCallService;
        public GetRollcallMetadataCommandHandler(IRollCallService rollCallService) 
        {
            _rollCallService = rollCallService;
        }

        public async Task<RollcallMetadata> Handle(GetRollcallMetadataCommand request, CancellationToken cancellationToken) 
        {
            return await _rollCallService.GetRollCallData(request.schoolId, request.date);
        }
    }
    
}
