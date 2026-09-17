using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.DemoApp.TickerQ;

public class MyBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    public MyBackgroundWorker([NotNull] AbpAsyncTimer timer, [NotNull] IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        timer.Period = 60 * 1000; // 60 seconds
        CronExpression = "* * * * *"; // every minute
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Console.WriteLine($"MyBackgroundWorker executed at {DateTime.Now}");
    }
}
