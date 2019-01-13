using Microsoft.Extensions.DependencyInjection;
using System;

namespace Payment.Service
{
    public class LoadApplication
    {
        public bool Start()
        {
            try
            {
                // create service collection
                var serviceCollection = new ServiceCollection();
                Startup.ConfigureServices(serviceCollection);

                // create service provider
                using (var serviceProvider = serviceCollection.BuildServiceProvider())
                {
                    // entry to run app
                    serviceProvider.GetService<Application>().Execute().Wait();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Stop()
        {
            Console.WriteLine("Stop");
            return true;
        }
    }
}