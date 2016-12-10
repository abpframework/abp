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
            using (var application = AbpApplication.Create<AbpDeskConsoleDemoModule>(services))
            {
                application.Initialize(services.BuildServiceProvider());

                application
                    .ServiceProvider
                    .GetRequiredService<TicketLister>()
                    .List();

                Console.ReadLine();
            }
        }
    }
}
