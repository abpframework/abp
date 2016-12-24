using System;
using Microsoft.Extensions.DependencyInjection;

namespace AbpDesk.ConsoleDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var application = services.AddApplication<AbpDeskConsoleDemoModule>();
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                application.Initialize(scope.ServiceProvider);

                application
                    .ServiceProvider
                    .GetRequiredService<TicketLister>()
                    .List();

                application
                    .ServiceProvider
                    .GetRequiredService<UserLister>()
                    .List();

                Console.ReadLine();

                application.Shutdown();
            }
        }
    }
}
