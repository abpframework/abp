using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public class MongoDbContextEventOutbox<TMongoDbContext> : IMongoDbContextEventOutbox<TMongoDbContext> 
        where TMongoDbContext : IHasEventOutbox
    {
        public Task EnqueueAsync(OutgoingEventInfo outgoingEvent)
        {
            throw new NotImplementedException();
        }

        public Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(int maxCount)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}