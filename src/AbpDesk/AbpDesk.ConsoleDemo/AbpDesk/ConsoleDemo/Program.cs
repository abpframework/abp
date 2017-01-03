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
            var application = services.AddApplication<AbpDeskConsoleDemoModule>();
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                application.Initialize(scope.ServiceProvider);

                RunDemo(application);

                Console.WriteLine("Press ENTER to run again...");
                Console.ReadLine();

                RunDemo(application);

                Console.WriteLine();
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();

                application.Shutdown();
            }
        }

        private static void RunDemo(AbpApplication application)
        {
            application
                .ServiceProvider
                .GetRequiredService<TicketLister>()
                .List();

            application
                .ServiceProvider
                .GetRequiredService<UserLister>()
                .List();

            application
                .ServiceProvider
                .GetRequiredService<BlogPostLister>()
                .List();
        }
    }
}
