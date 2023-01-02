using System;
using System.Threading;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.DemoApp.HangFire;

class Program
{
    static void Main(string[] args)
    {
        using (var application = AbpApplicationFactory.Create<DemoAppHangfireModule>(options =>
               {
                   options.UseAutofac();
               }))
        {
            application.Initialize();

            CancelableBackgroundJob(application.ServiceProvider);
            Console.WriteLine("Started: " + typeof(Program).Namespace);
            Console.WriteLine("Press ENTER to stop the application..!");
            Console.ReadLine();

            application.Shutdown();
        }
    }

    private static void CancelableBackgroundJob(IServiceProvider serviceProvider)
    {
        AsyncHelper.RunSync(async () =>
        {
            var backgroundJobManager = serviceProvider.GetRequiredService<IBackgroundJobManager>();
            var jobId = await backgroundJobManager.EnqueueAsync(new LongRunningJobArgs { Value = "test-1" });
            await backgroundJobManager.EnqueueAsync(new LongRunningJobArgs { Value = "test-2" });
            Thread.Sleep(1000);
            BackgroundJob.Delete(jobId);
        });
    }
}
