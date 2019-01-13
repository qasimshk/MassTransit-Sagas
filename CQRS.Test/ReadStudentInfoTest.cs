using CQRS.Business.Utils;
using CQRS.Models;
using CQRS.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Test
{
    [TestClass]
    public class ReadStudentInfoTest
    {
        private readonly AllStudentsInfoQueryHandler _allStudentsInfoQuery;
        private readonly DataRepository _dataRepository;

        public ReadStudentInfoTest()
        {
            _dataRepository = new DataRepository();
            var repository = _dataRepository.GetMock();

            _allStudentsInfoQuery = new AllStudentsInfoQueryHandler(_dataRepository.GetAutoMapperInstance(),
                repository.Object);
        }

        [TestMethod]        
        public async Task GetAllStudentInfoTest()
        {
            var obj = new AllStudentsInfoQuery();

            var resp = await _allStudentsInfoQuery.Handle(obj) as Results;

            Assert.IsNotNull(resp.ResponseMessage);

            Assert.AreEqual(resp.IsSuccessful, true);

            Assert.IsInstanceOfType(resp.ResponseMessage,
                typeof(IEnumerable<StudentsViewDto>));
        }

        [TestMethod]
        public async Task GetStudentInfoByCorrectIDTest()
        {
            var obj = new AllStudentsInfoQuery(1);

            var resp = await _allStudentsInfoQuery.Handle(obj) as Results;

            Assert.IsNotNull(resp.ResponseMessage);

            Assert.AreEqual(resp.IsSuccessful, true);

            Assert.AreEqual(1, resp.ResponseMessage.Count);

            Assert.IsInstanceOfType(resp.ResponseMessage,
                typeof(IEnumerable<StudentsViewDto>));
        }

        [TestMethod]
        public async Task PassingIncorrectStudentIdTest()
        {
            var obj = new AllStudentsInfoQuery(13);

            var resp = await _allStudentsInfoQuery.Handle(obj) as Results;

            Assert.AreEqual(false, resp.IsSuccessful);

            Assert.AreEqual(null, resp.ResponseMessage);
        }
    }
}