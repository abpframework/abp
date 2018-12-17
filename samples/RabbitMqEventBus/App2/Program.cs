using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace App2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<App2Module>())
            {
                application.Initialize();

                var messagingService = application
                    .ServiceProvider
                    .GetRequiredService<App2MessagingService>();

                messagingService.Run();

                application.Shutdown();
            }
        }
    }
}
