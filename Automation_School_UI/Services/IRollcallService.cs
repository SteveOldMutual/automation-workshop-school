using Automation_School_UI.Models;

namespace Automation_School_UI.Services
{
    public enum ROLLCALLOUTCOME { SUCCESS, ALREADYPRESENT, ALREADYABSENT, ALREADYDISMISSED, NOTPRESENT, ERROR , NOTDISMISSED }
    public interface IRollcallService
    {
        public Task<ROLLCALLOUTCOME> MarkPresent(string id);
        public Task<ROLLCALLOUTCOME> MarkAbsent(string id);

        public Task<ROLLCALLOUTCOME> DismissStudent(string id);
        public Task<ROLLCALLOUTCOME> UndoDismiss(string id);
        public Task<RollcallMetadata> GetRollCallData(DateTime dateTime);
        public Task<RollcallMetadata> GetStudentRollcallData(string id);

    }
}
