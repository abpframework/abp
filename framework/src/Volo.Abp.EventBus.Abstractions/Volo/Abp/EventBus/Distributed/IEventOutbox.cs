using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed;

public interface IEventOutbox
{
    Task EnqueueAsync(OutgoingEventInfo outgoingEvent);

    Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(int maxCount, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id);

    Task DeleteManyAsync(IEnumerable<Guid> ids);
}
