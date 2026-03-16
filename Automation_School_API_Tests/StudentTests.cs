using Automation_School_API.Commands.Authentication;
using Automation_School_API.Commands.Students;
using Automation_School_API.Models.Students;
using Automation_School_API_Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_School_API_Tests
{
    public class StudentTests : TestFixture
    {
        [Fact]
        public async Task CreateStudent() 
        {
            Student student = new Student()
            {  
                SchoolId = "1",
                FirstName = "CreateFirst",
                LastName = "CreateLast",
                Age = 12,
                Grade = "8"  ,
            };

            StudentDTO studentDTO = student.ToStudentDTO();

            var response = await Mediator.Send(new CreateStudentCommand(studentDTO));
            Assert.NotNull(response);
            response.FirstName.Should().Be(studentDTO.FirstName);
            response.LastName.Should().Be(studentDTO.LastName);
            response.Age.Should().Be(studentDTO.Age);
            response.Grade.Should().Be(studentDTO.Grade);

            //db validation
            var dbStudent = await Context.Students.FirstOrDefaultAsync(x => x.Id == response.Id);
            Assert.NotNull(dbStudent);
            dbStudent.FirstName.Should().Be(studentDTO.FirstName);
            dbStudent.LastName.Should().Be(studentDTO.LastName);
            dbStudent.Age.Should().Be(studentDTO.Age);
            dbStudent.Grade.Should().Be(studentDTO.Grade);
        }

        [Fact]
        public async Task GetStudent()
        {
            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "GetFirst",
                LastName = "GetLast",
                Age = 12,
                Grade = "8"
            };

            await InsertAsync(student);

            var response = await Mediator.Send(new GetStudentCommand(student.SchoolId, student.Id));
            Assert.NotNull(response);
            response.FirstName.Should().Be(student.FirstName);
            response.LastName.Should().Be(student.LastName);
            response.Age.Should().Be(student.Age);
            response.Grade.Should().Be(student.Grade);

        }

        [Fact]
        public async Task GetStudents()
        {
            Student student1 = new Student()
            {
                SchoolId = "1",
                FirstName = "GetFirst1",
                LastName = "GetLast1",
                Age = 12,
                Grade = "8"
            };

            Student student2 = new Student()
            {
                SchoolId = "1",
                FirstName = "GetFirst2",
                LastName = "GetLast2",
                Age = 12,
                Grade = "8"
            };

            Student[] students = [student1,student2];

            await InsertAsync(students);

            var response = await Mediator.Send(new GetStudentsCommand(student1.SchoolId));
            Assert.NotNull(response);
            response.FirstOrDefault(x => x.Id.Equals(student1.Id)).FirstName.Should().Be(student1.FirstName);
            response.FirstOrDefault(x => x.Id.Equals(student1.Id)).LastName.Should().Be(student1.LastName);
            response.FirstOrDefault(x => x.Id.Equals(student1.Id)).Grade.Should().Be(student1.Grade);
            response.FirstOrDefault(x => x.Id.Equals(student1.Id)).Age.Should().Be(student1.Age);

            response.FirstOrDefault(x => x.Id.Equals(student2.Id)).FirstName.Should().Be(student2.FirstName);
            response.FirstOrDefault(x => x.Id.Equals(student2.Id)).LastName.Should().Be(student2.LastName);
            response.FirstOrDefault(x => x.Id.Equals(student2.Id)).Grade.Should().Be(student2.Grade);
            response.FirstOrDefault(x => x.Id.Equals(student2.Id)).Age.Should().Be(student2.Age);


        }
        [Fact]
        public async Task UpdateStudent()
        {
            StudentDTO studentDTO = new StudentDTO()
            {
                SchoolId = "1",
                FirstName = "UpdatedFirst",
                LastName = "UpdatedLast",
                Age = 13,
                Grade = "7"
            };

            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "OriginalFirst",
                LastName = "OriginalLast",
                Age = 12,
                Grade = "8"
            };

            await InsertAsync(student);

            var response = await Mediator.Send(new UpdateStudentCommand(student.Id, studentDTO));
            Assert.NotNull(response);
            response.FirstName.Should().Be(studentDTO.FirstName);
            response.LastName.Should().Be(studentDTO.LastName);
            response.Age.Should().Be(studentDTO.Age);
            response.Grade.Should().Be(studentDTO.Grade);

        }
        [Fact]
        public async Task DeleteStudent()
        {
            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "DeleteFirst",
                LastName = "DeleteLast",
                Age = 12,
                Grade = "8"
            };

            await InsertAsync(student);
            var response = await Mediator.Send(new DeleteStudentCommand(student.Id));
            response.Should().BeTrue();
        }
        [Fact]
        public async Task DeleteNoExistingStudentReturnsFalse()
        {
            
            var response = await Mediator.Send(new DeleteStudentCommand("12"));
            response.Should().BeFalse();
        }

        public async Task GetStudentsFromOnlyOneSchool() 
        {
            Student student = new Student()
            {
                SchoolId = "1",
                FirstName = "FirstSchool",
                LastName = "OriginalLast",
                Age = 12,
                Grade = "8"
            };

            Student studentSchool2 = new Student()
            {
                SchoolId = "2",
                FirstName = "otherSchool",
                LastName = "OriginalLast",
                Age = 12,
                Grade = "8"
            };

            Student[] students = [student, studentSchool2];
            await InsertAsync(students);
        }
    }
}
