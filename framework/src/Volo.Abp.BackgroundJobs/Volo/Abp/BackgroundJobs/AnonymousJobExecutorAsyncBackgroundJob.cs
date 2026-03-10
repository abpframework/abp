using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs;

public class AnonymousJobExecutorAsyncBackgroundJob : AsyncBackgroundJob<AnonymousJobArgs>, ITransientDependency
{
    protected IAnonymousJobHandlerRegistry HandlerRegistry { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public AnonymousJobExecutorAsyncBackgroundJob(
        IAnonymousJobHandlerRegistry handlerRegistry,
        IServiceProvider serviceProvider,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        HandlerRegistry = handlerRegistry;
        ServiceProvider = serviceProvider;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public override async Task ExecuteAsync(AnonymousJobArgs args)
    {
        var handler = HandlerRegistry.Get(args.JobName);
        if (handler == null)
        {
            throw new AbpException("No anonymous job handler registered for: " + args.JobName);
        }

        await handler(args.JsonData, ServiceProvider, CancellationTokenProvider.Token);
    }
}
