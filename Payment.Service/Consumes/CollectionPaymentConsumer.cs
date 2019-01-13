using CQRS.Models.Payment;
using MassTransit;
using Payment.Service.Commands;
using Payment.Service.Helper;
using System;
using System.Threading.Tasks;

namespace Payment.Service.Consumes
{
    public class CollectionPaymentConsumer : IConsumer<IPaymentReceivedEvent>
    {
        private readonly Messages _messages;

        public CollectionPaymentConsumer(Messages messages)
        {
            _messages = messages;
        }

        public async Task Consume(ConsumeContext<IPaymentReceivedEvent> context)
        {
            var res = await _messages.DispatchAsync(new CollectPaymentCommand
            {
                StudentId = context.Message.StudentId,
                Amount = context.Message.Amount,
                CourseName = context.Message.CourseName,
                TransactionDate = context.Message.TransactionDate,                
                CorrelationId = (Guid)context.CorrelationId
            });

            if (!res.IsSuccessful)
                Console.WriteLine(res.ResponseMessage);
            else
            {
                await Console.Out.WriteLineAsync($"Payment £{context.Message.Amount} collected successfully");
                await Console.Out.WriteLineAsync($"Id:{context.CorrelationId}");
            }
            Console.WriteLine(string.Empty);
        }
    }
}