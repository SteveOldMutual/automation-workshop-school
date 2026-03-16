using Automation_School_API.Commands.Rollcall;
using Automation_School_API.Models.Students;
using Automation_School_API_Tests.Fixtures;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Automation_School_API_Tests
{
    public class RollcallTests : TestFixture
    {
        [Fact]
        public async Task StudentCanBeMarkedPresent() 
        {
            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "RollCall",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

            await InsertAsync(student);

            var response = await Mediator.Send(new SetPresentCommand(student.SchoolId,student.Id, true));
            response.Should().Be(ROLLCALLOUTCOME.SUCCESS);
            var rollCall = Context.RollCall.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date && x.studentId == student.Id);
            rollCall.Should().NotBeNull();
            rollCall.IsPresent.Should().BeTrue();
        }
        [Fact]
        public async Task StudentCannotBeMarkedPresentTwiceForTheSameDate()
        {
            Student student = new Student()
            {SchoolId = "1",
                FirstName = "Dismiss",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

            Rollcall rollcall = new()
            {
                SchoolId = "1",
                Dismissed = false,
                Date = DateTime.Now,
                IsPresent = true,
                studentId = student.Id
            };

            await InsertAsync(student);
            await InsertAsync(rollcall);

            var response = await Mediator.Send(new SetPresentCommand(student.SchoolId,student.Id,true));
            response.Should().Be(ROLLCALLOUTCOME.ALREADYPRESENT);
        }

        [Fact]
        public async Task AbsentStudentCanBeMarkedPresentOnTheSameDate()
        {
            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "RollCall",
                LastName = "Absent",
                Age = 12,
                Grade = "8"
            };

            Rollcall rollcall = new()
            {
                SchoolId = "1",
                Dismissed = false,
                Date = DateTime.Now,
                IsPresent = false,
                studentId = student.Id
            };

            await InsertAsync(student);
            await InsertAsync(rollcall);
            var response = await Mediator.Send(new SetPresentCommand(student.SchoolId,student.Id, true));
            response.Should().Be(ROLLCALLOUTCOME.SUCCESS);
            var rollCall = Context.RollCall.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date && x.studentId == student.Id);
            rollCall.Should().NotBeNull();
            rollCall.IsPresent.Should().BeTrue();
        }

        [Fact]
        public async Task StudentCanBeMarkedAbsent()
        {
            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "RollCall",
                LastName = "Absent",
                Age = 12,
                Grade = "8"
            };

            await InsertAsync(student);

            var response = await Mediator.Send(new SetPresentCommand(student.SchoolId,student.Id, false));
            response.Should().Be(ROLLCALLOUTCOME.SUCCESS);
            var rollCall = Context.RollCall.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date && x.studentId == student.Id);
            rollCall.Should().NotBeNull();
            rollCall.IsPresent.Should().BeFalse();
        }

        [Fact]
        public async Task PresentStudentCanBeDismissed()
        {
            Student student = new Student()
            {SchoolId = "1",
                FirstName = "Dismiss",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

            Rollcall rollcall = new()
            {
                SchoolId = "1",
                Dismissed = false,
                Date = DateTime.Now,
                IsPresent = true,
                studentId = student.Id
            }; 

            await InsertAsync(student);
            await InsertAsync(rollcall);

            var response = await Mediator.Send(new DismissStudentCommand(student.Id));
            response.Should().Be(ROLLCALLOUTCOME.SUCCESS);
            var rollCall = Context.RollCall.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date && x.studentId == student.Id);
            rollCall.Should().NotBeNull();
            rollCall.Dismissed.Should().BeTrue();
        }

        [Fact]
        public async Task DismissalCanBeUndone() 
        {
            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "Dismiss",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

            Rollcall rollcall = new()
            {
                SchoolId = "1",
                Dismissed = true,
                DismissedTimestamp = DateTime.Now,
                Date = DateTime.Now,
                IsPresent = true,
                studentId = student.Id
            };

            await InsertAsync(student);
            await InsertAsync(rollcall);

            var response = await Mediator.Send(new UndoDismissStudentCommand(student.Id));
            response.Should().Be(ROLLCALLOUTCOME.SUCCESS);
        }

        [Fact]
        public async Task CannotUndoWhenStudentNotDismissed()
        {
            Student student = new Student()
            {SchoolId = "1",
                FirstName = "Dismiss",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

            Rollcall rollcall = new()
            {SchoolId = "1",
                Dismissed = false,
                Date = DateTime.Now,
                IsPresent = true,
                studentId = student.Id
            };

            await InsertAsync(student);
            await InsertAsync(rollcall);

            var response = await Mediator.Send(new UndoDismissStudentCommand(student.Id));
            response.Should().Be(ROLLCALLOUTCOME.NOTDISMISSED);
        }

        [Fact]
        public async Task CannotUndoWhenStudentAbsent()
        {
            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "Dismiss",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

            Rollcall rollcall = new()
            {
                SchoolId="1",
                Dismissed = false,
                Date = DateTime.Now,
                IsPresent = false,
                studentId = student.Id
            };

            await InsertAsync(student);
            await InsertAsync(rollcall);

            var response = await Mediator.Send(new UndoDismissStudentCommand(student.Id));
            response.Should().Be(ROLLCALLOUTCOME.NOTPRESENT);
        }

        [Fact]
        public async Task UnMarkedStudentCannotBeDismissed() 
        {
            Student student = new Student()
            {SchoolId = "1",
                FirstName = "Dismiss",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

          

            await InsertAsync(student);

            var response = await Mediator.Send(new DismissStudentCommand(student.Id));
            response.Should().Be(ROLLCALLOUTCOME.NOTPRESENT);
        }

        [Fact]
        public async Task AbsentStudentCannotBeDismissed()
        {
            Student student = new Student()
            {
                SchoolId="1",
                FirstName = "Dismiss",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

            Rollcall rollcall = new()
            {
                SchoolId = "1",
                Dismissed = false,
                Date = DateTime.Now,
                IsPresent = false,
                studentId = student.Id
            };

            await InsertAsync(student);
            await InsertAsync(rollcall);

            var response = await Mediator.Send(new DismissStudentCommand(student.Id));
            response.Should().Be(ROLLCALLOUTCOME.NOTPRESENT);
            var rollCall = Context.RollCall.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date && x.studentId == student.Id);
            rollCall.Should().NotBeNull();
            rollCall.Dismissed.Should().BeFalse();
        }

        public async Task AddRollcall(string schoolId, DateTime date, bool present, bool dismissed) 
        {
            Student student = new Student()
            {
                SchoolId = schoolId,
                FirstName = "Dismiss",
                LastName = "Present",
                Age = 12,
                Grade = "8"
            };

            Rollcall rollcall = new()
            {

                SchoolId = schoolId,
                Dismissed = dismissed,
                Date =date,
                IsPresent = present,                
                studentId = student.Id
            };

            await InsertAsync(student);
            await InsertAsync(rollcall);
        }


        [Fact]
        public async Task RollCallTotalsCanBeRetrievedPerDate() 
        {
            var schoolId = "1";
            var date = DateTime.Now;
            await AddRollcall(schoolId,date,false, false);
            await AddRollcall(schoolId,date, true, true);
            await AddRollcall(schoolId,date, true, false);

            var response = await Mediator.Send(new GetRollcallMetadataCommand(schoolId,date));
            response.Rollcalls.Count().Should().BeGreaterThan(2);
            response.TotalPresent.Should().BeGreaterThan(0);
            response.TotalAbsent.Should().BeGreaterThan(0);
            response.TotalDismissed.Should().BeGreaterThan(0);
        }
        public async Task AddRollcall(string schoolId, Student student, DateTime date, bool present, bool dismissed)
        {
            

            Rollcall rollcall = new()
            {
                SchoolId = schoolId,
                Dismissed = dismissed,
                Date =date,
                IsPresent = present,
                studentId = student.Id
            };

            await InsertAsync(rollcall);
        }
        [Fact]
        public async Task AbleToRetrieveMetadataForStudent() 
        {
              var schoolId = "1";
            var date = DateTime.Now;

            Student student = new Student()
            {
                FirstName = "MetaData",
                LastName = "Student",
                Age = 12,
                Grade = "8"
            };

            await AddRollcall(schoolId, student,date.AddDays(1), false, false);
            await AddRollcall(schoolId, student, date.AddDays(2), true, true);
            await AddRollcall(schoolId, student, date.AddDays(3), true, false);

            var response = await Mediator.Send(new GetStudentRollcallDataCommand(schoolId, student.Id));
            response.Rollcalls.All(x => x.studentId == student.Id).Should().BeTrue();
            response.Rollcalls.Count().Should().BeGreaterThan(2);
            response.TotalPresent.Should().BeGreaterThan(0);
            response.TotalAbsent.Should().BeGreaterThan(0);
            response.TotalDismissed.Should().BeGreaterThan(0);
        }
    }
}
