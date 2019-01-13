using CQRS.Business.Utils;
using CQRS.Commands;
using CQRS.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace CQRS.Test
{
    [TestClass]
    public class StudentRegistrationTest
    {
        private readonly DataRepository _dataRepository;
        private readonly RegisterStudentInfoCommandHandler _registerStudentInfoCommandHandler;
        private readonly UnRegisteredStudentCommandHandler _unRegisteredStudentCommandHandler;

        public StudentRegistrationTest()
        {
            _dataRepository = new DataRepository();
            var repository = _dataRepository.GetMock();

            _registerStudentInfoCommandHandler = new RegisterStudentInfoCommandHandler(
                _dataRepository.GetAutoMapperInstance(), repository.Object);

            _unRegisteredStudentCommandHandler = new UnRegisteredStudentCommandHandler(
                repository.Object, _dataRepository.GetAutoMapperInstance());
        }

        [TestMethod]
        public async Task RegisterStudentTesting()
        {
            var obj = new RegisterStudentInfoCommand
            {
                FirstName = "Test",
                LastName = "Student",
                DOB = DateTime.Parse("1985-11-30"),
                Email = "TestStudent@abc.com"
            };

            var resp = await _registerStudentInfoCommandHandler.Handle(obj);

            Assert.AreEqual(true, resp.IsSuccessful);

            Assert.IsInstanceOfType(resp.ResponseMessage, typeof(Student));
        }

        [TestMethod]
        [Description("Register student with invalid DOB")]
        public async Task RegisterStudentWithInvalidDOBTesting()
        {
            var obj = new RegisterStudentInfoCommand
            {
                FirstName = "Test",
                LastName = "Student",
                DOB = DateTime.MinValue,
                Email = "TestStudent@abc.com"
            };

            var resp = await _registerStudentInfoCommandHandler.Handle(obj);

            Assert.AreEqual(false, resp.IsSuccessful);

            Assert.AreEqual(State.unProcessableEntity, resp.Status);
        }

        [TestMethod]
        [Description("Register student with invalid email")]
        public async Task InvalidEmailTesting()
        {
            var obj = new RegisterStudentInfoCommand
            {
                FirstName = "Test",
                LastName = "Student",
                DOB = DateTime.Parse("1985-11-30"),
                Email = "TestStudentsdfsd"
            };

            var resp = await _registerStudentInfoCommandHandler.Handle(obj);

            Assert.AreEqual(false, resp.IsSuccessful);

            Assert.AreEqual(State.unProcessableEntity, resp.Status);
        }

        [TestMethod]
        [Description("Required fields are missing")]
        public async Task RequiredFieldTesting()
        {
            var obj = new RegisterStudentInfoCommand
            {
                FirstName = string.Empty,
                LastName = "Student",
                DOB = DateTime.Parse("1985-11-30"),
                Email = string.Empty
            };

            var resp = await _registerStudentInfoCommandHandler.Handle(obj);

            Assert.AreEqual(false, resp.IsSuccessful);

            Assert.AreEqual(State.unProcessableEntity, resp.Status);
        }

        [TestMethod]
        [Description("string max length test")]
        public async Task MaxLengthofstringTesting()
        {
            var obj = new RegisterStudentInfoCommand
            {
                FirstName = "Test",
                LastName = "this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request ",
                DOB = DateTime.Parse("1985-11-30"),
                Email = "test@test.com"
            };

            var resp = await _registerStudentInfoCommandHandler.Handle(obj);

            Assert.AreEqual(false, resp.IsSuccessful);

            Assert.AreEqual(State.unProcessableEntity, resp.Status);

            StringAssert.EndsWith(resp.ResponseMessage, "50.");
        }

        [TestMethod]
        public async Task UnRegisterStudentTesting()
        {
            var obj = new UnRegisteredStudentCommand
            {
                StudentId = 1
            };

            var resp = await _unRegisteredStudentCommandHandler.Handle(obj);

            Assert.AreEqual(true, resp.IsSuccessful);

            Assert.AreEqual(State.noContent, resp.Status);
        }

        [TestMethod]
        public async Task UnRegisterNonExistingStudentTesting()
        {
            var obj = new UnRegisteredStudentCommand
            {
                StudentId = 16
            };

            var resp = await _unRegisteredStudentCommandHandler.Handle(obj);

            Assert.AreEqual(false, resp.IsSuccessful);

            Assert.AreEqual(State.notFound, resp.Status);
        }

        [TestMethod]
        public async Task NullRequestTesting()
        {
            var resp = await _unRegisteredStudentCommandHandler.Handle(null);

            Assert.AreEqual(false, resp.IsSuccessful);

            Assert.AreEqual(State.badRequest, resp.Status);
        }
    }
}
