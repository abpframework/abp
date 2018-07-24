using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs.DemoApp.Jobs;

namespace Volo.Abp.BackgroundJobs.DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<DemoAppModule>(options =>
            {
                options.UseAutofac();
            }))
            {
                application.Initialize();

                application
                    .ServiceProvider
                    .GetRequiredService<SampleJobCreator>()
                    .CreateJobs();

                Console.WriteLine("Press ENTER to stop the application..!");
                Console.ReadLine();

                application.Shutdown();
            }
        }
    }
}
