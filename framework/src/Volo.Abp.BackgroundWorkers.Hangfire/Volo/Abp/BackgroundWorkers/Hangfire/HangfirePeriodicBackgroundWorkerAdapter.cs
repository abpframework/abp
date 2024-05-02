using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

public class HangfirePeriodicBackgroundWorkerAdapter<TWorker> : HangfireBackgroundWorkerBase
    where TWorker : IBackgroundWorker
{
    private readonly MethodInfo _doWorkAsyncMethod;
    private readonly MethodInfo _doWorkMethod;

    public HangfirePeriodicBackgroundWorkerAdapter()
    {
        _doWorkAsyncMethod =
            typeof(TWorker).GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        _doWorkMethod = typeof(TWorker).GetMethod("DoWork", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public async override Task DoWorkAsync()
    {
        var workerContext = new PeriodicBackgroundWorkerContext(ServiceProvider);
        var worker = ServiceProvider.GetRequiredService<TWorker>();

        switch (worker)
        {
            case AsyncPeriodicBackgroundWorkerBase asyncPeriodicBackgroundWorker:
                await (Task)_doWorkAsyncMethod.Invoke(asyncPeriodicBackgroundWorker, new object[] { workerContext });
                break;
            case PeriodicBackgroundWorkerBase periodicBackgroundWorker:
                _doWorkMethod.Invoke(periodicBackgroundWorker, new object[] { workerContext });
                break;
        }
    }
}
