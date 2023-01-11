using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.DemoApp.HangFire;

class Program
{
    async static Task Main(string[] args)
    {
        using (var application = await AbpApplicationFactory.CreateAsync<DemoAppHangfireModule>(options =>
               {
                   options.UseAutofac();
               }))
        {
            await application.InitializeAsync();

            await CancelableBackgroundJobAsync(application.ServiceProvider);

            Console.WriteLine("Started: " + typeof(Program).Namespace);
            Console.WriteLine("Press ENTER to stop the application..!");
            Console.ReadLine();

            await application.ShutdownAsync();
        }
    }

    private async static Task CancelableBackgroundJobAsync(IServiceProvider serviceProvider)
    {
        var backgroundJobManager = serviceProvider.GetRequiredService<IBackgroundJobManager>();
        var jobId = await backgroundJobManager.EnqueueAsync(new LongRunningJobArgs { Value = "test-1" });
        await backgroundJobManager.EnqueueAsync(new LongRunningJobArgs { Value = "test-2" });
        Thread.Sleep(1000);
        BackgroundJob.Delete(jobId);
    }
}
