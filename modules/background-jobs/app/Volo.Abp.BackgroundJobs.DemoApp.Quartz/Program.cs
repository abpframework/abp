using System;

namespace Volo.Abp.BackgroundJobs.DemoApp.Quartz;

class Program
{
    static void Main(string[] args)
    {
        using (var application = AbpApplicationFactory.Create<DemoAppQuartzModule>(options =>
        {
            options.UseAutofac();
        }))
        {
            application.Initialize();

            Console.WriteLine("Started: " + typeof(Program).Namespace);
            Console.WriteLine("Press ENTER to stop the application..!");
            Console.ReadLine();

            application.Shutdown();
        }
    }
}
