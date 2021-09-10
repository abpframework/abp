using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public class DbContextEventInbox<TDbContext> : IDbContextEventInbox<TDbContext> 
        where TDbContext : IHasEventInbox
    {
        public Task EnqueueAsync(IncomingEventInfo incomingEvent)
        {
            throw new NotImplementedException();
        }

        public Task<List<IncomingEventInfo>> GetWaitingEventsAsync(int maxCount)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsProcessedAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByMessageIdAsync(string messageId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOldEventsAsync()
        {
            throw new NotImplementedException();
        }
    }
}