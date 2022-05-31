using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus.Boxes;

public class InboxProcessManager : IBackgroundWorker
{
    protected AbpDistributedEventBusOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected List<IInboxProcessor> Processors { get; }

    public InboxProcessManager(
        IOptions<AbpDistributedEventBusOptions> options,
        IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        Processors = new List<IInboxProcessor>();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (var inboxConfig in Options.Inboxes.Values)
        {
            if (inboxConfig.IsProcessingEnabled)
            {
                var processor = await GetInboxProcessorAsync(inboxConfig);
                await processor.StartAsync(inboxConfig, cancellationToken);
                Processors.Add(processor);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (var processor in Processors)
        {
            await processor.StopAsync(cancellationToken);
        }
    }

    protected async Task<IInboxProcessor> GetInboxProcessorAsync(InboxConfig inboxConfig)
    {
        var providers = ServiceProvider.GetRequiredService<IEnumerable<IInboxProcessorProvider>>();

        foreach (var provider in providers)
        {
            var processor = await provider.GetOrNullAsync(inboxConfig);

            if (processor is not null)
            {
                return processor;
            }
        }

        throw new AbpException($"Inbox processor for {inboxConfig.Name} not found!");
    }
}
