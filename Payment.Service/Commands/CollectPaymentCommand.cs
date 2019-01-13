using CQRS.Business;
using CQRS.Business.Messaging;
using CQRS.Business.Utils;
using CQRS.Models;
using CQRS.Models.Notification;
using CQRS.Models.Payment;
using Payment.Service.Business;
using System;
using System.Threading.Tasks;

namespace Payment.Service.Commands
{
    public class CollectPaymentCommand : ICommand, IPayment
    {
        public Guid CorrelationId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
    }

    public class CollectPaymentCommandHandler : Results,
        ICommandHandler<CollectPaymentCommand>
    {
        private readonly IRepository _repository;
        private readonly IBusConfig _busConfig;

        public CollectPaymentCommandHandler(IRepository repository,
            IBusConfig busConfig)
        {
            _repository = repository;
            _busConfig = busConfig;
        }

        public async Task<Results> Handle(CollectPaymentCommand command)
        {
            var student = await _repository.GetStudentByID(command.StudentId);

            if (student == null)
                return SetResponse(false, "Un-Registered Student");

            command.StudentName = student.StudentName;
            return SetResponse(true);
        }
    }
}