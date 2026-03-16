using Automation_School_API.Models.Students;

namespace Automation_School_API.Domain.SchoolManagement
{
    public interface IRollCallService
    {
        public Task<ROLLCALLOUTCOME> MarkStudentPresent(string schoolId,string id);
        public Task<ROLLCALLOUTCOME> MarkStudentAbsent(string schoolId,string id);
        public Task<ROLLCALLOUTCOME> DismissStudent(string id);
        public Task<ROLLCALLOUTCOME> UndoDismiss(string id);

        public Task<Rollcall> GetStudentRollcall (string id, DateTime dateTime);
        public Task<RollcallMetadata> GetRollCallData(string schoolId, DateTime dateTime);
        public Task<RollcallMetadata> GetRollCallData(string schoolId, string id);



    }
}
