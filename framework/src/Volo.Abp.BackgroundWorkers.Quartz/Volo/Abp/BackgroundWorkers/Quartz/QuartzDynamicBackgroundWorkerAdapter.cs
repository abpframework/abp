using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.BackgroundWorkers.Quartz;

public class QuartzDynamicBackgroundWorkerAdapter : IJob, ITransientDependency
{
    protected IDynamicBackgroundWorkerHandlerRegistry HandlerRegistry { get; }
    protected IServiceProvider ServiceProvider { get; }
    public ILogger<QuartzDynamicBackgroundWorkerAdapter> Logger { get; set; }

    public QuartzDynamicBackgroundWorkerAdapter(
        IDynamicBackgroundWorkerHandlerRegistry handlerRegistry,
        IServiceProvider serviceProvider)
    {
        HandlerRegistry = handlerRegistry;
        ServiceProvider = serviceProvider;
        Logger = NullLogger<QuartzDynamicBackgroundWorkerAdapter>.Instance;
    }

    public virtual async Task Execute(IJobExecutionContext context)
    {
        var rawWorkerName = context.MergedJobDataMap.GetString(QuartzDynamicBackgroundWorkerManager.DynamicWorkerNameKey);
        if (string.IsNullOrWhiteSpace(rawWorkerName))
        {
            return;
        }

        var workerName = rawWorkerName!;
        var handler = HandlerRegistry.Get(workerName);
        if (handler == null)
        {
            Logger.LogWarning("No handler registered for dynamic worker: {WorkerName}", workerName);
            return;
        }

        try
        {
            await handler(
                new DynamicBackgroundWorkerExecutionContext(workerName, ServiceProvider),
                context.CancellationToken);
        }
        catch (Exception ex)
        {
            // Swallow the exception to match the behavior of AsyncPeriodicBackgroundWorkerBase,
            // which catches, notifies and logs without rethrowing. This prevents Quartz from
            // treating a single failed execution as a job failure and triggering retries.
            await ServiceProvider.GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(new ExceptionNotificationContext(ex));

            Logger.LogException(ex);
        }
    }
}
