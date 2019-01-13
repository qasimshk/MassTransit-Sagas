using AutoMapper;
using CQRS.Business;
using CQRS.Business.Utils;
using CQRS.Models;
using CQRS.Repositories;
using System.Threading.Tasks;

namespace CQRS.Queries
{
    public sealed class StudentCoursesInfoQuery : IQuery<object>
    {
        public int _studentId { get; private set; }

        public StudentCoursesInfoQuery(int studentId)
        {
            _studentId = studentId;
        }
    }

    public sealed class StudentCoursesInfoQueryHandler : Results,
        IQueryHandler<StudentCoursesInfoQuery, object>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentCoursesInfoQueryHandler(IMapper mapper,
            IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<dynamic> Handle(StudentCoursesInfoQuery query)
        {
            var student = await _studentRepository.GetStudentByIdAsync(query._studentId);
            if (student == null)
                return SetResponse(State.notFound, false, "Student not found");

            var courses = await _studentRepository.GetCoursesByStudent(query._studentId);
            return SetResponse(State.ok, true, _mapper.Map<StudentCoursesDto>(courses));
        }
    }
}