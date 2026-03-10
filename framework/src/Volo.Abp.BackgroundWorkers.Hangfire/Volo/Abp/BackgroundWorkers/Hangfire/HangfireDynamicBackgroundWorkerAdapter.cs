using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

public class HangfireDynamicBackgroundWorkerAdapter : ITransientDependency
{
    protected IDynamicBackgroundWorkerHandlerRegistry DynamicBackgroundWorkerHandlerRegistry { get; }
    protected IServiceProvider ServiceProvider { get; }

    public HangfireDynamicBackgroundWorkerAdapter(
        IDynamicBackgroundWorkerHandlerRegistry dynamicBackgroundWorkerHandlerRegistry,
        IServiceProvider serviceProvider)
    {
        DynamicBackgroundWorkerHandlerRegistry = dynamicBackgroundWorkerHandlerRegistry;
        ServiceProvider = serviceProvider;
    }

    public virtual async Task DoWorkAsync(string workerName, CancellationToken cancellationToken = default)
    {
        var handler = DynamicBackgroundWorkerHandlerRegistry.Get(workerName);
        if (handler == null)
        {
            return;
        }

        await handler(new DynamicBackgroundWorkerExecutionContext(workerName, ServiceProvider), cancellationToken);
    }
}
