using CQRS.Business;
using CQRS.Business.Messaging;
using CQRS.Business.Utils;
using CQRS.Models;
using CQRS.Models.Notification;
using CQRS.Models.Payment;
using MassTransit;
using Payment.Service.Business;
using Payment.Service.Helper;
using System;
using System.Threading.Tasks;

namespace Payment.Service.Commands
{
    public class RefundPaymentConsumer : IConsumer<IPaymentRefund>
    {
        private readonly Messages _messages;

        public RefundPaymentConsumer(Messages messages)
        {
            _messages = messages;
        }

        public async Task Consume(ConsumeContext<IPaymentRefund> context)
        {
            var res = await _messages.DispatchAsync(new RefundPaymentCommand
            {
                StudentId = context.Message.StudentId,
                Amount = context.Message.Amount,
                CourseName = context.Message.CourseName,
                TransactionDate = context.Message.TransactionDate
            });

            if (!res.IsSuccessful)
                Console.WriteLine(res.ResponseMessage);
            else
                Console.WriteLine("Payment refunded successfully");
        }
    }

    public class RefundPaymentCommand : ICommand, IPaymentRefund
    {
        public Guid CorrelationId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
    }

    public class RefundPaymentCommandHandler : Results,
        ICommandHandler<RefundPaymentCommand>
    {
        private readonly IRepository _repository;
        private readonly IBusConfig _busConfig;

        public RefundPaymentCommandHandler(IRepository repository,
            IBusConfig busConfig)
        {
            _repository = repository;
            _busConfig = busConfig;
        }

        public async Task<Results> Handle(RefundPaymentCommand command)
        {
            var student = await _repository.GetStudentByID(command.StudentId);

            if (student == null)
                return SetResponse(false, "Un-Registered Student");

            command.StudentName = student.StudentName;
            //await _busConfig.PublichAsync(Queues.Notifications, new Notification
            //{
            //    Message = $"{command.StudentName} amount has been refunded. " +
            //    $"Refunded amount: £{command.Amount}"
            //});
            return SetResponse(true);
        }
    }
}