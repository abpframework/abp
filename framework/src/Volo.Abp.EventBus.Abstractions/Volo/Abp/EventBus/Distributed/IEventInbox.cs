using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed;

public interface IEventInbox
{
    Task EnqueueAsync(IncomingEventInfo incomingEvent);

    Task<List<IncomingEventInfo>> GetWaitingEventsAsync(int maxCount, Expression<Func<IIncomingEventInfo, bool>>? filter = null, CancellationToken cancellationToken = default);

    Task MarkAsProcessedAsync(Guid id);

    Task RetryLaterAsync(Guid id, int retryCount, DateTime? nextRetryTime);

    Task MarkAsDiscardAsync(Guid id);

    Task<bool> ExistsByMessageIdAsync(string messageId);

    Task DeleteOldEventsAsync();
}
