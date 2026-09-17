using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs;

public class DynamicBackgroundJobExecutorJob : AsyncBackgroundJob<DynamicBackgroundJobArgs>, ITransientDependency
{
    protected IDynamicBackgroundJobHandlerRegistry HandlerRegistry { get; }
    protected IServiceProvider ServiceProvider { get; }

    public DynamicBackgroundJobExecutorJob(
        IDynamicBackgroundJobHandlerRegistry handlerRegistry,
        IServiceProvider serviceProvider)
    {
        HandlerRegistry = handlerRegistry;
        ServiceProvider = serviceProvider;
    }

    public override async Task ExecuteAsync(DynamicBackgroundJobArgs args)
    {
        Logger.LogDebug(
            "Executing dynamic job. TransportJobName: {TransportJobName}, EffectiveJobName: {EffectiveJobName}",
            DynamicBackgroundJobArgs.JobNameConstant,
            args.JobName
        );

        var handler = HandlerRegistry.Get(args.JobName);
        if (handler == null)
        {
            throw new AbpException(
                $"No dynamic job handler registered for: {args.JobName}. " +
                $"The handler may have been unregistered or the application restarted since the job was enqueued.");
        }

        var cancellationToken = ServiceProvider.GetRequiredService<ICancellationTokenProvider>().Token;
        var executionContext = new DynamicBackgroundJobExecutionContext(args.JobName, args.JsonData, ServiceProvider);
        await handler(executionContext, cancellationToken);
    }
}
