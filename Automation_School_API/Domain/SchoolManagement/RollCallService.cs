
using Automation_School_API.Context;
using Automation_School_API.Models.Students;
using Microsoft.EntityFrameworkCore;

namespace Automation_School_API.Domain.SchoolManagement
{
    public class RollCallService : IRollCallService
    {
        private readonly IConfiguration _configuration;
        private readonly SchoolDBContext _schoolDBContext;
        public RollCallService(IConfiguration config, SchoolDBContext context)
        {
            _configuration = config;
            _schoolDBContext = context;
        }

        public async Task<ROLLCALLOUTCOME> DismissStudent(string id)
        {
            DateTime now = DateTime.Now;
           var student =  await _schoolDBContext.RollCall.FirstOrDefaultAsync(x => x.studentId == id && x.Date.Date == now.Date);
            if (student == null || !student.IsPresent) 
            {
                return ROLLCALLOUTCOME.NOTPRESENT;
            }

            if (student.Dismissed) return ROLLCALLOUTCOME.ALREADYDISMISSED;

            student.Dismissed = true;
            student.DismissedTimestamp = now.Date;

            await _schoolDBContext.SaveChangesAsync();

            return ROLLCALLOUTCOME.SUCCESS;

        }

        public Task<RollcallMetadata> GetRollCallData(string schoolId, DateTime dateTime)
        {
            DateTime now = DateTime.Now;
            List<Rollcall> rollcalls = _schoolDBContext.RollCall.Where(x => x.Date.Date == now.Date && x.SchoolId == schoolId).ToList();

            RollcallMetadata metadata = new RollcallMetadata() 
            {
                RollCallDate = now.Date,
                Rollcalls = rollcalls,
                TotalAbsent = rollcalls.Where(x => !x.IsPresent).Count(),
                TotalPresent = rollcalls.Where(x => x.IsPresent).Count(),
                TotalDismissed = rollcalls.Where(x => x.Dismissed).Count()
            };

            return Task.FromResult(metadata);
        }

        public Task<RollcallMetadata> GetRollCallData(string schoolId, string id)
        {
            List<Rollcall> rollcalls = _schoolDBContext.RollCall.Where(x => x.studentId == id && x.SchoolId == schoolId).ToList();

            RollcallMetadata metadata = new RollcallMetadata()
            {
                RollCallDate = DateTime.Now.Date,
                Rollcalls = rollcalls,
                TotalAbsent = rollcalls.Where(x => !x.IsPresent).Count(),
                TotalPresent = rollcalls.Where(x => x.IsPresent).Count(),
                TotalDismissed = rollcalls.Where(x => x.Dismissed).Count()
            };

            return Task.FromResult(metadata);
        }

        public async Task<Rollcall> GetStudentRollcall(string id, DateTime dateTime)
        {
            DateTime now = DateTime.Now;
            Rollcall rollcall = await _schoolDBContext.RollCall.FirstOrDefaultAsync(x => x.studentId == id && x.Date.Date == now.Date);
            return rollcall;
        }

        public async Task<ROLLCALLOUTCOME> MarkStudentAbsent(string schoolId,string id)
        {
            DateTime now = DateTime.Now;
            Rollcall rollcall = await _schoolDBContext.RollCall.FirstOrDefaultAsync(x => x.studentId == id && x.Date.Date == now.Date);

            if (rollcall == null)
            {
                rollcall = new()
                {
                    SchoolId = schoolId,
                    IsPresent = false,
                    Dismissed = false,
                    Date = now.Date,
                    studentId = id
                };
                await _schoolDBContext.RollCall.AddAsync(rollcall);
                await _schoolDBContext.SaveChangesAsync();
                return ROLLCALLOUTCOME.SUCCESS;
            }

            if (rollcall.Dismissed)
            {
                return ROLLCALLOUTCOME.ALREADYDISMISSED;
            }

            if (rollcall.IsPresent)
            {
                rollcall.IsPresent = false;
                rollcall.Date = now.Date;
                await _schoolDBContext.SaveChangesAsync();
                return ROLLCALLOUTCOME.SUCCESS;
            }

            if (!rollcall.IsPresent)
            {
                return ROLLCALLOUTCOME.ALREADYABSENT;
            }            

            return ROLLCALLOUTCOME.ERROR;
        }

        public async Task<ROLLCALLOUTCOME> MarkStudentPresent(string schoolId,string id)
        {
            DateTime now = DateTime.Now;
            Rollcall rollcall = await _schoolDBContext.RollCall.FirstOrDefaultAsync(x => x.studentId == id && x.Date.Date == now.Date);

            if (rollcall == null) 
            {
                rollcall = new()
                {
                    SchoolId = schoolId,
                    IsPresent = true,
                    Dismissed = false,
                    Date = now.Date,
                    studentId = id
                };
                await _schoolDBContext.RollCall.AddAsync(rollcall);
                await _schoolDBContext.SaveChangesAsync();
                return ROLLCALLOUTCOME.SUCCESS;
            }

            if (rollcall.Dismissed)
            {
                return ROLLCALLOUTCOME.ALREADYDISMISSED;
            }

            if (!rollcall.IsPresent) 
            {
                rollcall.IsPresent = true;
                rollcall.Date = now.Date;
                await _schoolDBContext.SaveChangesAsync();
                return ROLLCALLOUTCOME.SUCCESS;
            }

            if (rollcall.IsPresent)
            {
                return ROLLCALLOUTCOME.ALREADYPRESENT;
            }

           

            return ROLLCALLOUTCOME.ERROR;
        }

        public async Task<ROLLCALLOUTCOME> UndoDismiss(string id)
        {
            DateTime now = DateTime.Now;
            Rollcall rollcall = await _schoolDBContext.RollCall.FirstOrDefaultAsync(x => x.studentId == id && x.Date.Date == now.Date);

            if (rollcall == null || !rollcall.IsPresent) return ROLLCALLOUTCOME.NOTPRESENT;

            if (!rollcall.Dismissed) return ROLLCALLOUTCOME.NOTDISMISSED;

            rollcall.Dismissed = false;
            rollcall.DismissedTimestamp = now.Date;

            await _schoolDBContext.SaveChangesAsync();
            return ROLLCALLOUTCOME.SUCCESS;

        }
    }
}
