using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
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
        protected AbpDistributedEventBusOptions DistributedEventsOptions { get; }
        protected IClock Clock { get; }

        public MongoDbContextEventInbox(
            IMongoDbContextProvider<TMongoDbContext> dbContextProvider,
            IClock clock,
            IOptions<AbpDistributedEventBusOptions> distributedEventsOptions)
        {
            DbContextProvider = dbContextProvider;
            Clock = clock;
            DistributedEventsOptions = distributedEventsOptions.Value;
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
        public virtual async Task<List<IncomingEventInfo>> GetWaitingEventsAsync(int maxCount, CancellationToken cancellationToken = default)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync(cancellationToken);

            var outgoingEventRecords = await dbContext
                .IncomingEvents
                .AsQueryable()
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreationTime)
                .Take(maxCount)
                .ToListAsync(cancellationToken: cancellationToken);

            return outgoingEventRecords
                .Select(x => x.ToIncomingEventInfo())
                .ToList();
        }

        [UnitOfWork]
        public async Task MarkAsProcessedAsync(Guid id)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            var incomingEvent = await dbContext.IncomingEvents.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (incomingEvent != null)
            {
                incomingEvent.MarkAsProcessed(Clock.Now);

                if (dbContext.SessionHandle != null)
                {
                    await dbContext.IncomingEvents.ReplaceOneAsync(dbContext.SessionHandle, Builders<IncomingEventRecord>.Filter.Eq(e => e.Id, incomingEvent.Id), incomingEvent);
                }
                else
                {
                    await dbContext.IncomingEvents.ReplaceOneAsync(Builders<IncomingEventRecord>.Filter.Eq(e => e.Id, incomingEvent.Id), incomingEvent);
                }
            }
        }

        [UnitOfWork]
        public async Task<bool> ExistsByMessageIdAsync(string messageId)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            return await dbContext.IncomingEvents.AsQueryable().AnyAsync(x => x.MessageId == messageId);
        }

        [UnitOfWork]
        public async Task DeleteOldEventsAsync()
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            var timeToKeepEvents = Clock.Now.Add(DistributedEventsOptions.InboxKeepEventTimeSpan);

            if (dbContext.SessionHandle != null)
            {
                await dbContext.IncomingEvents.DeleteManyAsync(dbContext.SessionHandle, x => x.Processed && x.CreationTime < timeToKeepEvents);
            }
            else
            {
                await dbContext.IncomingEvents.DeleteManyAsync(x => x.Processed && x.CreationTime < timeToKeepEvents);
            }
        }
    }
}
