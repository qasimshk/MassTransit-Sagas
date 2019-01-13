using CQRS.Business.Messaging;
using CQRS.Models;
using CQRS.Models.Payment;
using MassTransit;
using Payment.Service.Commands;
using Payment.Service.Consumes;
using Payment.Service.Helper;
using System;
using System.Threading.Tasks;

namespace Payment.Service
{
    public class Application
    {
        private readonly IBusConfig _busConfig;
        private readonly Messages _messages;

        public Application(IBusConfig busConfig, Messages messages)
        {
            _busConfig = busConfig;
            _messages = messages;
        }

        public async Task Execute()
        {
            var bus = _busConfig.ConfigureBus((cfg, host) =>
            {
                // procesing payment
                cfg.ReceiveEndpoint(host, nameof(IPaymentReceivedEvent), c =>
                {
                    c.Consumer(() => new CollectionPaymentConsumer(_messages));
                });

                // refund payment
                //cfg.ReceiveEndpoint(host, Queues.PaymentRefund, r =>
                //{
                //    r.Consumer(() => new RefundPaymentConsumer(_messages));
                //});
            });

            await bus.StartAsync();

            Console.WriteLine("Payment service is online");
            Console.WriteLine(string.Empty);
            Console.ReadLine();

            await bus.StopAsync();
        }
    }
}