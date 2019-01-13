using System;
using Topshelf;

namespace Payment.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Payment";

            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<LoadApplication>(Srv =>
                {
                    Srv.ConstructUsing(name => new LoadApplication());
                    Srv.WhenStarted(execute => execute.Start());
                    Srv.WhenStopped(execute => execute.Stop());
                });

                serviceConfig.SetServiceName("PaymentService");
                serviceConfig.SetDisplayName("Payment Service");
                serviceConfig.SetDescription("This is a service processing all the payments made by students");
                serviceConfig.StartAutomatically();
            });
        }
    }
}