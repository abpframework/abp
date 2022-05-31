using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Boxes;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents;

public class MongoDbInboxProcessorProvider : IInboxProcessorProvider, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public MongoDbInboxProcessorProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual Task<IInboxProcessor> GetOrNullAsync(InboxConfig inboxConfig)
    {
        return inboxConfig.ImplementationType.IsAssignableTo<IHasEventInbox>()
            ? Task.FromResult(ServiceProvider.GetRequiredService<IInboxProcessor>())
            : null;
    }
}