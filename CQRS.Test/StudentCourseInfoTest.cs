using CQRS.Models;
using CQRS.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace CQRS.Test
{
    [TestClass]
    public class StudentCourseInfoTest
    {
        private readonly DataRepository _dataRepository;
        private readonly StudentCoursesInfoQueryHandler _studentCoursesInfoQuery;

        public StudentCourseInfoTest()
        {
            _dataRepository = new DataRepository();
            var repository = _dataRepository.GetMock();

            _studentCoursesInfoQuery = new StudentCoursesInfoQueryHandler(_dataRepository.GetAutoMapperInstance(),
                repository.Object);
        }

        [TestMethod]
        public async Task GetStudentCoursesByStudentId()
        {
            var obj = new StudentCoursesInfoQuery(1);

            var resp = await _studentCoursesInfoQuery.Handle(obj);

            Assert.IsNotNull(resp.ResponseMessage);

            Assert.AreEqual(resp.IsSuccessful, true);

            Assert.IsInstanceOfType(resp.ResponseMessage,
                typeof(StudentCoursesDto));

            Assert.AreEqual(((StudentCoursesDto)resp
                .ResponseMessage).Courses.Count, 3);

            Assert.AreEqual(34,((StudentCoursesDto)resp
                .ResponseMessage).student.Age);
        }

        [TestMethod]
        [Description("Test student who is not enrolled in any course")]
        public async Task CheckStudentNotEnrolled()
        {
            var obj = new StudentCoursesInfoQuery(2);

            var resp = await _studentCoursesInfoQuery.Handle(obj);

            Assert.IsNotNull(resp.ResponseMessage);

            Assert.AreEqual(0,((StudentCoursesDto)resp
                .ResponseMessage).Courses.Count);
        }

        [TestMethod]
        public async Task NonExistingStudent()
        {            
            var obj = new StudentCoursesInfoQuery(34);

            var resp = await _studentCoursesInfoQuery.Handle(obj);

            Assert.AreEqual(resp.IsSuccessful, false);

            StringAssert.Contains(resp.ResponseMessage, "Student not found");
        }
    }
}