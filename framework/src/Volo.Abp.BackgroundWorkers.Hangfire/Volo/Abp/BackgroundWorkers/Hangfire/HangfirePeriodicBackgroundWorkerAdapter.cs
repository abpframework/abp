using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

public class HangfirePeriodicBackgroundWorkerAdapter<TWorker> : HangfireBackgroundWorkerBase
    where TWorker : IBackgroundWorker
{
    private readonly MethodInfo _doWorkAsyncMethod;
    private readonly MethodInfo _doWorkMethod;

    public HangfirePeriodicBackgroundWorkerAdapter(IOptions<AbpHangfirePeriodicBackgroundWorkerAdapterOptions> options)
    {
        TimeZone = options.Value.TimeZone;
        Queue = options.Value.Queue;
        RecurringJobId = BackgroundWorkerNameAttribute.GetNameOrNull<TWorker>();

        _doWorkAsyncMethod = typeof(TWorker).GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic)!;
        _doWorkMethod = typeof(TWorker).GetMethod("DoWork", BindingFlags.Instance | BindingFlags.NonPublic)!;
    }

    public override async Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        var workerContext = new PeriodicBackgroundWorkerContext(ServiceProvider, cancellationToken);
        var worker = ServiceProvider.GetRequiredService<TWorker>();

        switch (worker)
        {
            case AsyncPeriodicBackgroundWorkerBase asyncPeriodicBackgroundWorker:
                await (Task)(_doWorkAsyncMethod.Invoke(asyncPeriodicBackgroundWorker, [workerContext])!);
                break;
            case PeriodicBackgroundWorkerBase periodicBackgroundWorker:
                _doWorkMethod.Invoke(periodicBackgroundWorker, [workerContext]);
                break;
        }
    }
}
