using System;
using System.Threading.Tasks;
using Quartz;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Quartz;

public class QuartzDynamicBackgroundWorkerAdapter : IJob, ITransientDependency
{
    protected IDynamicBackgroundWorkerHandlerRegistry DynamicBackgroundWorkerHandlerRegistry { get; }
    protected IServiceProvider ServiceProvider { get; }

    public QuartzDynamicBackgroundWorkerAdapter(
        IDynamicBackgroundWorkerHandlerRegistry dynamicBackgroundWorkerHandlerRegistry,
        IServiceProvider serviceProvider)
    {
        DynamicBackgroundWorkerHandlerRegistry = dynamicBackgroundWorkerHandlerRegistry;
        ServiceProvider = serviceProvider;
    }

    public virtual async Task Execute(IJobExecutionContext context)
    {
        var workerName = context.MergedJobDataMap.GetString(QuartzBackgroundWorkerManager.DynamicWorkerNameKey);
        if (string.IsNullOrWhiteSpace(workerName))
        {
            return;
        }

        var nonNullWorkerName = workerName!;
        var handler = DynamicBackgroundWorkerHandlerRegistry.Get(nonNullWorkerName);
        if (handler == null)
        {
            return;
        }

        await handler(
            new DynamicBackgroundWorkerExecutionContext(nonNullWorkerName, ServiceProvider),
            context.CancellationToken
        );
    }
}
