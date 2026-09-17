using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed;

public interface IEventOutbox
{
    Task EnqueueAsync(OutgoingEventInfo outgoingEvent);

    Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(int maxCount, Expression<Func<IOutgoingEventInfo, bool>>? filter = null, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id);

    Task DeleteManyAsync(IEnumerable<Guid> ids);
}
