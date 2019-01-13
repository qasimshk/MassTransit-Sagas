using AutoMapper;
using CQRS.Business;
using CQRS.Business.Utils;
using CQRS.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CQRS.Commands
{
    public sealed class UnRegisteredStudentCommand : ICommand
    {
        [Required]
        public int StudentId { get; set; }
    }

    public class UnRegisteredStudentCommandHandler : CommandValidation, 
        ICommandHandler<UnRegisteredStudentCommand>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public UnRegisteredStudentCommandHandler(IStudentRepository studentRepository,
            IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<Results> Handle(UnRegisteredStudentCommand command)
        {
            if (command == null)
                return SetResponse(State.badRequest, false, null);

            var model = TryValidateCommand(command);
            if (!model.IsSuccessful)
                return model;

            var entity = await _studentRepository.GetStudentByIdAsync(command.StudentId);
            if (entity == null)
                return SetResponse(State.notFound, false, "Student not found");

            await _studentRepository.DeleteStudentAsync(entity);
            return SetResponse(State.noContent, true, null);
        }
    }
}