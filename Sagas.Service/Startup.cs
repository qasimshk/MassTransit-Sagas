using CQRS.Business.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sagas.Service
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
                "rabbitmq://localhost/", "guest", "guest"));            
        }
    }
}
