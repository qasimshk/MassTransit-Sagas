using AutoMapper;
using CQRS.Business;
using CQRS.Business.Utils;
using CQRS.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CQRS.Commands
{
    public sealed class EditStudentInfoCommand : ICommand
    {
        public int StudentId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
    }

    public class EditStudentInfoCommandHandler : CommandValidation, 
        ICommandHandler<EditStudentInfoCommand>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public EditStudentInfoCommandHandler(IMapper mapper,
            IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<Results> Handle(EditStudentInfoCommand command)
        {
            if(command == null)
                return SetResponse(State.badRequest, false, null);

            var model = TryValidateCommand(command);
            if (!model.IsSuccessful)
                return model;

            var dbstudent = await _studentRepository.GetStudentByIdAsync(command.StudentId);
            if (dbstudent == null)
                return SetResponse(State.notFound,false, "Student not found");
            
            await _studentRepository.UpdateStudentAsync(
                _mapper.Map(command, dbstudent));

            return SetResponse(State.noContent, true, null);
        }
    }
}