using CQRS.Business.Messaging;
using CQRS.Models;
using CQRS.Models.Payment;
using MassTransit;
using MassTransit.Saga;
using Sagas.Service.Payment;
using System;
using System.Threading.Tasks;

namespace Sagas.Service
{
    public class Application
    {
        private readonly IBusConfig _busConfig;
        private readonly PaymentSaga _paymentSaga;
        private readonly InMemorySagaRepository<PaymentSagaStatus> _repo;

        public Application(IBusConfig busConfig)
        {
            _busConfig = busConfig;
            _paymentSaga = new PaymentSaga();
            _repo = new InMemorySagaRepository<PaymentSagaStatus>();
        }

        public async Task Execute()
        {
            var bus = _busConfig.ConfigureBus((cfg, host) =>
            {
                // procesing payment
                cfg.ReceiveEndpoint(host, nameof(SubmitPayment), c =>
                {                    
                    c.StateMachineSaga(_paymentSaga, _repo);
                });
            });

            await bus.StartAsync();

            Console.WriteLine("Saga service is online");
            Console.WriteLine(string.Empty);
            Console.ReadLine();

            await bus.StopAsync();
        }
    }
}