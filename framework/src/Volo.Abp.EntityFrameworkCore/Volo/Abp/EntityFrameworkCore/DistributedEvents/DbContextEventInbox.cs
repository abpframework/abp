using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class DbContextEventInbox<TDbContext> : IDbContextEventInbox<TDbContext>
        where TDbContext : IHasEventInbox
    {
        protected IDbContextProvider<TDbContext> DbContextProvider { get; }
        protected AbpDistributedEventBusOptions DistributedEventsOptions { get; }
        protected IClock Clock { get; }

        public DbContextEventInbox(
            IDbContextProvider<TDbContext> dbContextProvider,
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

            dbContext.IncomingEvents.Add(
                new IncomingEventRecord(incomingEvent)
            );
        }

        [UnitOfWork]
        public virtual async Task<List<IncomingEventInfo>> GetWaitingEventsAsync(int maxCount, CancellationToken cancellationToken = default)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();

            var outgoingEventRecords = await dbContext
                .IncomingEvents
                .AsNoTracking()
                .Where(x => !x.Processed)
                .OrderBy(x => x.CreationTime)
                .Take(maxCount)
                .ToListAsync(cancellationToken: cancellationToken);

            return outgoingEventRecords
                .Select(x => x.ToIncomingEventInfo())
                .ToList();
        }

        [UnitOfWork]
        public virtual async Task MarkAsProcessedAsync(Guid id)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            var tableName = dbContext.IncomingEvents.EntityType.GetSchemaQualifiedTableName();

            var sql = $"UPDATE {tableName} SET Processed = 1, ProcessedTime = '{Clock.Now}' WHERE Id = '{id}'";
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }

        [UnitOfWork]
        public virtual async Task<bool> ExistsByMessageIdAsync(string messageId)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            return await dbContext.IncomingEvents.AnyAsync(x => x.MessageId == messageId);
        }

        [UnitOfWork]
        public virtual async Task DeleteOldEventsAsync()
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            var tableName = dbContext.IncomingEvents.EntityType.GetSchemaQualifiedTableName();
            var timeToKeepEvents = Clock.Now.Add(DistributedEventsOptions.InboxKeepEventTimeSpan);

            var sql = $"DELETE FROM {tableName} WHERE Processed = 1 AND CreationTime < '{timeToKeepEvents}'";
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
