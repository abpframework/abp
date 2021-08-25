using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace DistDemoApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<DistDemoAppModule>(opts =>
            {
                opts.UseAutofac();
            }))
            {
                application.Initialize();

                var demoService = application.ServiceProvider.GetRequiredService<DemoService>();
                await demoService.CreateTodoItemAsync();

                //Console.WriteLine("Press ENTER to exit");
                //Console.ReadLine();
                
                application.Shutdown();
            }
        }
    }
}