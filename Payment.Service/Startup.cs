using CQRS.Business;
using CQRS.Business.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Service.Business;
using Payment.Service.Commands;
using Payment.Service.Helper;

namespace Payment.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Application>();

            services.AddTransient<IBusConfig>(b => new BusConfiguration(
                "rabbitmq://localhost/","guest", "guest"));

            // Repository
            services.AddTransient<IRepository, Repository>();

            // Command
            services.AddTransient<ICommandHandler<CollectPaymentCommand>,
                CollectPaymentCommandHandler>();

            services.AddTransient<ICommandHandler<RefundPaymentCommand>,
                RefundPaymentCommandHandler>();

            services.AddSingleton<Messages>();
        }
    }
}