using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

public class HangfireDynamicBackgroundWorkerAdapter : ITransientDependency
{
    protected IDynamicBackgroundWorkerHandlerRegistry HandlerRegistry { get; }
    protected IServiceProvider ServiceProvider { get; }
    public ILogger<HangfireDynamicBackgroundWorkerAdapter> Logger { get; set; }

    public HangfireDynamicBackgroundWorkerAdapter(
        IDynamicBackgroundWorkerHandlerRegistry handlerRegistry,
        IServiceProvider serviceProvider)
    {
        HandlerRegistry = handlerRegistry;
        ServiceProvider = serviceProvider;
        Logger = NullLogger<HangfireDynamicBackgroundWorkerAdapter>.Instance;
    }

    public virtual async Task DoWorkAsync(string workerName, CancellationToken cancellationToken = default)
    {
        var handler = HandlerRegistry.Get(workerName);
        if (handler == null)
        {
            Logger.LogWarning("No handler registered for dynamic worker: {WorkerName}", workerName);
            return;
        }

        try
        {
            await handler(new DynamicBackgroundWorkerExecutionContext(workerName, ServiceProvider), cancellationToken);
        }
        catch (Exception ex)
        {
            await ServiceProvider.GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(new ExceptionNotificationContext(ex));

            throw;
        }
    }
}
