using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TickerQ.Utilities.Models;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

public class AbpTickerQPeriodicBackgroundWorkerInvoker
{
    private readonly Func<AsyncPeriodicBackgroundWorkerBase, PeriodicBackgroundWorkerContext, Task>? _doWorkAsyncDelegate;
    private readonly Action<PeriodicBackgroundWorkerBase, PeriodicBackgroundWorkerContext>? _doWorkDelegate;

    protected IBackgroundWorker Worker { get; }
    protected IServiceProvider ServiceProvider { get; }

    public AbpTickerQPeriodicBackgroundWorkerInvoker(IBackgroundWorker worker, IServiceProvider serviceProvider)
    {
        Worker = worker;
        ServiceProvider = serviceProvider;

        switch (worker)
        {
            case AsyncPeriodicBackgroundWorkerBase:
            {
                var workerType = worker.GetType();
                var method = workerType.GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);
                if (method == null)
                {
                    throw new AbpException($"Could not find 'DoWorkAsync' method on type '{workerType.FullName}'.");
                }

                var instanceParam = Expression.Parameter(typeof(AsyncPeriodicBackgroundWorkerBase), "worker");
                var contextParam = Expression.Parameter(typeof(PeriodicBackgroundWorkerContext), "context");
                var call = Expression.Call(Expression.Convert(instanceParam, workerType), method, contextParam);
                var lambda = Expression.Lambda<Func<AsyncPeriodicBackgroundWorkerBase, PeriodicBackgroundWorkerContext, Task>>(call, instanceParam, contextParam);
                _doWorkAsyncDelegate = lambda.Compile();
                break;
            }
            case PeriodicBackgroundWorkerBase:
            {
                var workerType = worker.GetType();
                var method = workerType.GetMethod("DoWork", BindingFlags.Instance | BindingFlags.NonPublic);
                if (method == null)
                {
                    throw new AbpException($"Could not find 'DoWork' method on type '{workerType.FullName}'.");
                }

                var instanceParam = Expression.Parameter(typeof(PeriodicBackgroundWorkerBase), "worker");
                var contextParam = Expression.Parameter(typeof(PeriodicBackgroundWorkerContext), "context");
                var call = Expression.Call(Expression.Convert(instanceParam, workerType), method, contextParam);
                var lambda = Expression.Lambda<Action<PeriodicBackgroundWorkerBase, PeriodicBackgroundWorkerContext>>(call, instanceParam, contextParam);
                _doWorkDelegate = lambda.Compile();
                break;
            }
        }
    }

    public virtual async Task DoWorkAsync(TickerFunctionContext context, CancellationToken cancellationToken = default)
    {
        var workerContext = new PeriodicBackgroundWorkerContext(ServiceProvider);
        switch (Worker)
        {
            case AsyncPeriodicBackgroundWorkerBase asyncPeriodicBackgroundWorker:
                await _doWorkAsyncDelegate!(asyncPeriodicBackgroundWorker, workerContext);
                break;
            case PeriodicBackgroundWorkerBase periodicBackgroundWorker:
                _doWorkDelegate!(periodicBackgroundWorker, workerContext);
                break;
        }
    }
}
