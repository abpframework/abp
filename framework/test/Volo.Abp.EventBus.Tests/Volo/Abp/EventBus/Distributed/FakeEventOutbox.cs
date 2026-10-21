using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Distributed;

public class FakeEventOutbox : IEventOutbox, ISingletonDependency
{
    public List<OutgoingEventInfo> Events { get; } = new();

    public Task EnqueueAsync(OutgoingEventInfo outgoingEvent)
    {
        Events.Add(outgoingEvent);
        return Task.CompletedTask;
    }

    public Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(
        int maxCount,
        Expression<Func<IOutgoingEventInfo, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Events.Take(maxCount).ToList());
    }

    public Task DeleteAsync(Guid id)
    {
        Events.RemoveAll(x => x.Id == id);
        return Task.CompletedTask;
    }

    public Task DeleteManyAsync(IEnumerable<Guid> ids)
    {
        var idList = ids.ToList();
        Events.RemoveAll(x => idList.Contains(x.Id));
        return Task.CompletedTask;
    }
}
