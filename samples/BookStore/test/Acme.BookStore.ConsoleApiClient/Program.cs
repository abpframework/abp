using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Threading;

namespace Acme.BookStore.ConsoleApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<ConsoleApiClientModule>(options =>
            {
                options.UseAutofac();
            }))
            {
                application.Initialize();

                using (var scope = application.ServiceProvider.CreateScope())
                {
                    var demoService = scope.ServiceProvider.GetRequiredService<ApiClientDemoService>();
                    AsyncHelper.RunSync(() => demoService.RunAsync());
                }

                Console.WriteLine("Press ENTER to stop application...");
                Console.ReadLine();
            }
        }
    }
}
