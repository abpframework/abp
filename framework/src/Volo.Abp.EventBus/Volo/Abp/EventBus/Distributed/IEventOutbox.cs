using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed
{
    public interface IEventOutbox
    {
        Task EnqueueAsync(OutgoingEventInfo outgoingEvent);
        
        Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(int maxCount);
        
        Task DeleteAsync(Guid id);
    }
}