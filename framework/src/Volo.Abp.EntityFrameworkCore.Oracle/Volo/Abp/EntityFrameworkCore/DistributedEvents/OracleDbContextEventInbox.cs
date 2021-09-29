using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class OracleDbContextEventInbox<TDbContext> : DbContextEventInbox<TDbContext> , IOracleDbContextEventInbox<TDbContext>
        where TDbContext : IHasEventInbox
    {
        public OracleDbContextEventInbox(
            IDbContextProvider<TDbContext> dbContextProvider,
            IClock clock,
            IOptions<AbpDistributedEventBusOptions> distributedEventsOptions) : base(dbContextProvider, clock, distributedEventsOptions)
        {
        }

        [UnitOfWork]
        public override async Task MarkAsProcessedAsync(Guid id)
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            var tableName = dbContext.IncomingEvents.EntityType.GetSchemaQualifiedTableName();

            var sql = $"UPDATE \"{tableName}\" SET \"Processed\" = '1', \"ProcessedTime\" = TO_DATE('{Clock.Now}', 'yyyy-mm-dd hh24:mi:ss') WHERE \"Id\" = HEXTORAW('{GuidToOracleType(id)}')";
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }

        [UnitOfWork]
        public override async Task DeleteOldEventsAsync()
        {
            var dbContext = await DbContextProvider.GetDbContextAsync();
            var tableName = dbContext.IncomingEvents.EntityType.GetSchemaQualifiedTableName();
            var timeToKeepEvents = Clock.Now.Add(DistributedEventsOptions.InboxKeepEventTimeSpan);

            var sql = $"DELETE FROM \"{tableName}\" WHERE \"Processed\" = '1' AND \"CreationTime\" < TO_DATE('{timeToKeepEvents}', 'yyyy-mm-dd hh24:mi:ss')";
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }

        protected virtual string GuidToOracleType(Guid id)
        {
            return BitConverter.ToString(id.ToByteArray()).Replace("-", "").ToUpper();
        }
    }
}
