using Topshelf;

namespace Sagas.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<LoadApplication>(Srv =>
                {
                    Srv.ConstructUsing(name => new LoadApplication());
                    Srv.WhenStarted(execute => execute.Start());
                    Srv.WhenStopped(execute => execute.Stop());
                });

                serviceConfig.SetServiceName("SagaService");
                serviceConfig.SetDisplayName("Saga Service");
                serviceConfig.SetDescription("This is a service processing all saga events");
                serviceConfig.StartAutomatically();
            });
        }
    }
}