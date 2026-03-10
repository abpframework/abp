using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs;

public class AnonymousJobExecutorAsyncBackgroundJob : AsyncBackgroundJob<AnonymousJobArgs>, ITransientDependency
{
    protected IAnonymousJobHandlerRegistry HandlerRegistry { get; }
    protected IServiceProvider ServiceProvider { get; }

    public AnonymousJobExecutorAsyncBackgroundJob(
        IAnonymousJobHandlerRegistry handlerRegistry,
        IServiceProvider serviceProvider)
    {
        HandlerRegistry = handlerRegistry;
        ServiceProvider = serviceProvider;
    }

    public override async Task ExecuteAsync(AnonymousJobArgs args)
    {
        Logger.LogInformation(
            "Executing anonymous transport job. TransportJobName: {TransportJobName}, EffectiveJobName: {EffectiveJobName}",
            AnonymousJobArgs.JobNameConstant,
            args.JobName
        );

        var handler = HandlerRegistry.Get(args.JobName);
        if (handler == null)
        {
            throw new AbpException("No anonymous job handler registered for: " + args.JobName);
        }

        var cancellationToken = ServiceProvider.GetRequiredService<ICancellationTokenProvider>().Token;
        var executionContext = new AnonymousJobExecutionContext(args.JobName, args.JsonData, ServiceProvider);
        await handler(executionContext, cancellationToken);
    }
}
