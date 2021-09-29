using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class DbContextEventOutbox<TDbContext> : IDbContextEventOutbox<TDbContext>
        where TDbContext : IHasEventOutbox
    {
        protected IDbContextProvider<TDbContext> DbContextProvider { get; }
        protected AbpEfCoreDistributedEventBusOptions EfCoreDistributedEventBusOptions { get; }

        public DbContextEventOutbox(
            IDbContextProvider<TDbContext> dbContextProvider,
            IOptions<AbpEfCoreDistributedEventBusOptions> efCoreDistributedEventBusOptions)
        {
            DbContextProvider = dbContextProvider;
            EfCoreDistributedEventBusOptions = efCoreDistributedEventBusOptions.Value;
        }

        [UnitOfWork]
        public virtual async Task EnqueueAsync(OutgoingEventInfo outgoingEvent)
        {
            var dbContext = (IHasEventOutbox) await DbContextProvider.GetDbContextAsync();
            dbContext.OutgoingEvents.Add(
                new OutgoingEventRecord(outgoingEvent)
            );
        }

        [UnitOfWork]
        public virtual async Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(int maxCount, CancellationToken cancellationToken = default)
        {
            var dbContext = (IHasEventOutbox) await DbContextProvider.GetDbContextAsync();

            var outgoingEventRecords = await dbContext
                .OutgoingEvents
                .AsNoTracking()
                .OrderBy(x => x.CreationTime)
                .Take(maxCount)
                .ToListAsync(cancellationToken: cancellationToken);

            return outgoingEventRecords
                .Select(x => x.ToOutgoingEventInfo())
                .ToList();
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(Guid id)
        {
            var dbContext = (IHasEventOutbox) await DbContextProvider.GetDbContextAsync();
            var tableName = dbContext.OutgoingEvents.EntityType.GetSchemaQualifiedTableName();
            var connectionName = dbContext.Database.GetDbConnection().GetType().Name.ToLower();
            var sqlAdapter = EfCoreDistributedEventBusOptions.GetSqlAdapter(connectionName);

            var sql = $"DELETE FROM {sqlAdapter.NormalizeTableName(tableName)} WHERE " +
                      $"{sqlAdapter.NormalizeColumnNameEqualsValue("Id", id)}";

            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
