using AutoMapper;
using CQRS.Business;
using CQRS.Business.Utils;
using CQRS.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CQRS.Commands
{
    public sealed class PartialEditStudentInfoCommand
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
    }

    public class PartialEditStudentInfoCommandHandler : CommandValidation,
        ICommandPatchHandler<JsonPatchDocument<PartialEditStudentInfoCommand>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public PartialEditStudentInfoCommandHandler(IMapper mapper,
            IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        public async Task<Results> Handle(JsonPatchDocument<PartialEditStudentInfoCommand> command, dynamic Id)
        {
            if(command == null)
                return SetResponse(State.badRequest, false, null);

            var dbstudent = await _studentRepository.GetStudentByIdAsync(Id);
            if (dbstudent == null)
                return SetResponse(State.notFound, false, "Student not found");

            var studentToPatch = _mapper.Map<PartialEditStudentInfoCommand>(dbstudent);

            command.ApplyTo(studentToPatch);

            var model = TryValidateCommand(studentToPatch);
            if (!model.IsSuccessful)
                return model;

            if (studentToPatch.FirstName == studentToPatch.LastName)
                return SetResponse(State.unProcessableEntity, false,
                    "First name and last name cannot be same");

            await _studentRepository.UpdateStudentAsync(
                _mapper.Map(studentToPatch, dbstudent));

            return SetResponse(State.noContent, true, null);
        }
    }
}