using AutoMapper;
using CQRS.Business;
using CQRS.Business.Utils;
using CQRS.Models;
using CQRS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CQRS.Entities;

namespace CQRS.Queries
{
    public sealed class AllStudentsInfoQuery : IQuery<object>
    {
        public int _studentId { get; private set; }

        public AllStudentsInfoQuery()
        {

        }

        public AllStudentsInfoQuery(int StudentId)
        {
            _studentId = StudentId;
        }
    }

    public sealed class AllStudentsInfoQueryHandler : Results,
        IQueryHandler<AllStudentsInfoQuery, object>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public AllStudentsInfoQueryHandler(IMapper mapper,
            IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<dynamic> Handle(AllStudentsInfoQuery query)
        {            
            List<Student> resp = query._studentId > 0
                ? (dynamic)await _studentRepository.GetStudentFilterByIdAsync(query._studentId)
                : (dynamic)await _studentRepository.GetAllStudentsAsync();

            if (resp.First() == null)
                return SetResponse(State.notFound, false, null);

            return SetResponse(State.ok, true, 
                _mapper.Map<IEnumerable<StudentsViewDto>>(resp));
        }
    }
}