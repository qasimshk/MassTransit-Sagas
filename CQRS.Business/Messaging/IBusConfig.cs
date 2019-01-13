using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Threading.Tasks;

namespace CQRS.Business.Messaging
{
    public interface IBusConfig
    {
        IBusControl ConfigureBus(
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>
                registrationAction = null);

        Task PublichAsync<T>(string queue, T value) where T : class;
    }
}