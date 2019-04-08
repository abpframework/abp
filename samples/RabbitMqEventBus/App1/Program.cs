using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace App1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<App1Module>(options =>
            {
                options.UseAutofac();
            }))
            {
                application.Initialize();

                var messagingService = application
                    .ServiceProvider
                    .GetRequiredService<App1MessagingService>();

                messagingService.Run();

                application.Shutdown();
            }
        }
    }
}
