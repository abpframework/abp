using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Boxes;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public class EfCoreInboxProcessorProvider : IInboxProcessorProvider, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public EfCoreInboxProcessorProvider(IServiceProvider serviceProvider)
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