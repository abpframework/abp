using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus.Boxes;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public class PostgreSqlDbContextEventInbox<TDbContext> : DbContextEventInbox<TDbContext>, IPostgreSqlDbContextEventInbox<TDbContext>
    where TDbContext : IHasEventInbox
{
    public PostgreSqlDbContextEventInbox(
        IDbContextProvider<TDbContext> dbContextProvider,
        IClock clock,
        IOptions<AbpEventBusBoxesOptions> eventBusBoxesOptions)
        : base(dbContextProvider, clock, eventBusBoxesOptions)
    {
    }

    [UnitOfWork]
    public override async Task MarkAsProcessedAsync(Guid id)
    {
        var dbContext = await DbContextProvider.GetDbContextAsync();
        var tableName = dbContext.IncomingEvents.EntityType.GetSchemaQualifiedTableName();

        var sql = $"UPDATE \"{tableName}\" SET \"Processed\" = '1', \"ProcessedTime\" = '{Clock.Now}' WHERE \"Id\" = '{id}'";
        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }

    [UnitOfWork]
    public override async Task DeleteOldEventsAsync()
    {
        var dbContext = await DbContextProvider.GetDbContextAsync();
        var tableName = dbContext.IncomingEvents.EntityType.GetSchemaQualifiedTableName();
        var timeToKeepEvents = Clock.Now - EventBusBoxesOptions.WaitTimeToDeleteProcessedInboxEvents;

        var sql = $"DELETE FROM \"{tableName}\" WHERE \"Processed\" = '1' AND \"CreationTime\" < '{timeToKeepEvents}'";
        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }
}
