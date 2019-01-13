using Topshelf;

namespace Notification.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<LoadApplication>(Srv =>
                {
                    Srv.ConstructUsing(name => new LoadApplication());
                    Srv.WhenStarted(execute => execute.Start());
                    Srv.WhenStopped(execute => execute.Stop());
                });

                serviceConfig.SetServiceName("NotificationService");
                serviceConfig.SetDisplayName("Notification Service");
                serviceConfig.SetDescription("This service will notify the end user about the activity.");
                serviceConfig.StartAutomatically();
            });
        }
    }
}
