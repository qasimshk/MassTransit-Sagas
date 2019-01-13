using CQRS.Business.Utils;
using CQRS.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace CQRS.Test
{
    [TestClass]
    public class EditStudentInfoTest
    {
        private readonly DataRepository _dataRepository;
        private readonly EditStudentInfoCommandHandler _editStudentInfoCommandHandler;

        public EditStudentInfoTest()
        {
            _dataRepository = new DataRepository();
            var repository = _dataRepository.GetMock();

            _editStudentInfoCommandHandler = new EditStudentInfoCommandHandler(
                _dataRepository.GetAutoMapperInstance(), repository.Object);
        }

        [TestMethod]
        public async Task ModifyStudentDetailsTest()
        {
            var obj = new EditStudentInfoCommand
            {
                StudentId = 1,
                LastName = "Test User"
            };

            var res = await _editStudentInfoCommandHandler.Handle(obj) as Results;

            Assert.AreEqual(true, res.IsSuccessful);
        }

        [TestMethod]
        public async Task TestEditStudentInfoCommandWithNullObject()
        {
            var res = await _editStudentInfoCommandHandler.Handle(null) as Results;

            Assert.AreEqual(false, res.IsSuccessful);

            Assert.AreEqual(State.badRequest, res.Status);
        }

        [TestMethod]
        [Description("sending invalid studentId")]
        public async Task TestEditStudentInfoCommandPassingInvalidStudentId()
        {
            var obj = new EditStudentInfoCommand
            {
                StudentId = 235,
                LastName = "Invalid student"
            };

            var res = await _editStudentInfoCommandHandler.Handle(obj) as Results;

            Assert.AreEqual(false, res.IsSuccessful);

            StringAssert.Contains(res.ResponseMessage, "Student not found");

            Assert.AreEqual(State.notFound, res.Status);
        }

        [TestMethod]
        [Description("Validation testing")]
        public async Task TestValidationOfCommandObject()
        {
            var obj = new EditStudentInfoCommand
            {
                StudentId = 1,
                LastName = "this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request " +
                "this is lot of text to make bad request this is lot of text to make bad request this is lot of text to make bad request "
            };

            var res = await _editStudentInfoCommandHandler.Handle(obj) as Results;

            Assert.AreEqual(false, res.IsSuccessful);

            StringAssert.EndsWith(res.ResponseMessage, "50.");

            Assert.AreEqual(State.unProcessableEntity, res.Status);
        }        
    }
}