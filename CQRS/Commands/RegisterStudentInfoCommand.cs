using AutoMapper;
using CQRS.Business;
using CQRS.Business.Utils;
using CQRS.Entities;
using CQRS.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CQRS.Commands
{
    public sealed class RegisterStudentInfoCommand : ICommand
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
    }

    public class RegisterStudentInfoCommandHandler : CommandValidation, ICommandHandler<RegisterStudentInfoCommand>
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public RegisterStudentInfoCommandHandler(IMapper mapper,
            IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<Results> Handle(RegisterStudentInfoCommand command)
        {
            if(command == null)
                return SetResponse(State.badRequest, false, null);

            var model = TryValidateCommand(command);
            if (!model.IsSuccessful)
                return model;

            if (command.DOB == DateTime.MinValue)
                return SetResponse(State.unProcessableEntity, false, "Invalid DOB");

            var check = await _studentRepository.GetStudentByEmailAsync(command.Email);
            if (check != null)
                return SetResponse(State.badRequest, false, 
                    "Student with this email address is already registered");
            
            var entity = _mapper.Map<Student>(command);
            await _studentRepository.CreateStudentAsync(entity);

            return SetResponse(State.ok, true, entity);
        }
    }
}