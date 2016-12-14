using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace AbpDesk.ConsoleDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var application = AbpApplication.Create<AbpDeskConsoleDemoModule>(services);

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                application.Initialize(scope.ServiceProvider);

                application
                    .ServiceProvider
                    .GetRequiredService<TicketLister>()
                    .List();

                Console.ReadLine();

                application.Shutdown();
            }
        }
    }
}
