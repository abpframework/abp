using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed
{
    public interface IEventInbox
    {
        Task EnqueueAsync(IncomingEventInfo incomingEvent);
        
        Task<List<IncomingEventInfo>> GetWaitingEventsAsync(int maxCount);
        
        Task MarkAsProcessedAsync(Guid id);
    }
}