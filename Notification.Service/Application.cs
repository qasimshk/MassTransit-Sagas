using CQRS.Business.Messaging;
using CQRS.Models;
using CQRS.Models.Notification;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Notification.Service
{
    public class Application
    {
        private readonly IBusConfig _busConfig;

        public Application(IBusConfig busConfig)
        {
            _busConfig = busConfig;
        }

        public async Task Execute()
        {
            var bus = _busConfig.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, nameof(INotification), n =>
                {
                    n.Consumer<ReceiveMessageConsumer>();
                });
            });

            await bus.StartAsync();

            Console.WriteLine("Notification service is online");
            Console.WriteLine(string.Empty);
            Console.ReadLine();

            await bus.StopAsync();
        }
    }
}