using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Threading.Tasks;

namespace CQRS.Business.Messaging
{
    public class BusConfiguration : IBusConfig
    {
        private readonly string _mqUri;
        private readonly string _userName;
        private readonly string _password;

        public BusConfiguration(string MqUri, string UserName, string Password)
        {
            _mqUri = MqUri;
            _userName = UserName;
            _password = Password;
        }

        public IBusControl ConfigureBus(
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>
                registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(_mqUri), hst =>
                {
                    hst.Username(_userName);
                    hst.Password(_password);
                });
                registrationAction?.Invoke(cfg, host);
            });
        }

        public async Task PublichAsync<T>(string queue, T value) where T : class
        {
            var sendToUri = new Uri($"{_mqUri}{queue}");
            var endPoint = await ConfigureBus().GetSendEndpoint(sendToUri);
            await endPoint.Send(value);
        }
    }
}
