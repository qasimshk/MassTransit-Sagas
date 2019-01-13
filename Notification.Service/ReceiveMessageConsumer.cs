using CQRS.Models.Notification;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Notification.Service
{
    public class ReceiveMessageConsumer : IConsumer<INotification>
    {
        public async Task Consume(ConsumeContext<INotification> context)
        {
            await Console.Out.WriteLineAsync($"{context.Message.Message}");
            await Console.Out.WriteLineAsync($"Id:{context.CorrelationId}");
            Console.WriteLine(string.Empty);
        }
    }
}