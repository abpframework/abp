using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public class MongoDbContextEventInbox<TMongoDbContext> : IMongoDbContextEventInbox<TMongoDbContext> 
        where TMongoDbContext : IHasEventInbox
    {
        protected IMongoDbContextProvider<TMongoDbContext> DbContextProvider { get; }
        protected IClock Clock { get; }
        
        public MongoDbContextEventInbox(
            IMongoDbContextProvider<TMongoDbContext> dbContextProvider,
            IClock clock)
        {
            DbContextProvider = dbContextProvider;
            Clock = clock;
        }

        
        [UnitOfWork]
        public virtual async Task EnqueueAsync(IncomingEventInfo incomingEvent)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            if (dbContext.SessionHandle != null)
            {
                await dbContext.IncomingEvents.InsertOneAsync(
                    dbContext.SessionHandle,
                    new IncomingEventRecord(incomingEvent)
                );
            }
            else
            {
                await dbContext.IncomingEvents.InsertOneAsync(
                    new IncomingEventRecord(incomingEvent)
                );
            }
        }

        [UnitOfWork]
        public virtual async Task<List<IncomingEventInfo>> GetWaitingEventsAsync(int maxCount)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();

            var outgoingEventRecords = await dbContext
                .IncomingEvents
                .AsQueryable()
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreationTime)
                .Take(maxCount)
                .ToListAsync();
            
            return outgoingEventRecords
                .Select(x => x.ToIncomingEventInfo())
                .ToList();
        }

        [UnitOfWork]
        public async Task MarkAsProcessedAsync(Guid id)
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