using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus.Boxes;

public class OutboxSenderManager : IBackgroundWorker
{
    protected AbpDistributedEventBusOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected List<IOutboxSender> Senders { get; }

    public OutboxSenderManager(
        IOptions<AbpDistributedEventBusOptions> options,
        IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;
        Senders = new List<IOutboxSender>();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (var outboxConfig in Options.Outboxes.Values)
        {
            if (outboxConfig.IsSendingEnabled)
            {
                var sender = await GetOutboxSenderAsync(outboxConfig);
                await sender.StartAsync(outboxConfig, cancellationToken);
                Senders.Add(sender);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (var sender in Senders)
        {
            await sender.StopAsync(cancellationToken);
        }
    }

    protected async Task<IOutboxSender> GetOutboxSenderAsync(OutboxConfig outboxConfig)
    {
        var providers = ServiceProvider.GetRequiredService<IEnumerable<IOutboxSenderProvider>>();

        foreach (var provider in providers)
        {
            var sender = await provider.GetOrNullAsync(outboxConfig);

            if (sender is not null)
            {
                return sender;
            }
        }

        throw new AbpException($"Outbox sender for {outboxConfig.Name} not found!");
    }
}
