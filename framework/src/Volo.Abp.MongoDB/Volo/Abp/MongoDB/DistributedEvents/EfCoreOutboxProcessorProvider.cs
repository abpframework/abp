using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Boxes;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents;

public class MongoDbOutboxSenderProvider : IOutboxSenderProvider, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public MongoDbOutboxSenderProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual Task<IOutboxSender> GetOrNullAsync(OutboxConfig outboxConfig)
    {
        return outboxConfig.ImplementationType.IsAssignableTo<IHasEventOutbox>()
            ? Task.FromResult(ServiceProvider.GetRequiredService<IOutboxSender>())
            : null;
    }
}