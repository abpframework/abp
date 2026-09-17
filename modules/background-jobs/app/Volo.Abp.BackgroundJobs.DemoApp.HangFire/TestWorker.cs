using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.DemoApp.HangFire;

public class TestWorker : AsyncPeriodicBackgroundWorkerBase
{
    public TestWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory)
        : base(timer, serviceScopeFactory)
    {
        CronExpression = Cron.Minutely();
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Console.WriteLine($"[{DateTime.Now}] TestWorker executed.");
    }
}
