using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public class PostgreSqlDbContextEventOutbox<TDbContext> : DbContextEventOutbox<TDbContext>, IPostgreSqlDbContextEventOutbox<TDbContext>
    where TDbContext : IHasEventOutbox
{
    public PostgreSqlDbContextEventOutbox(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    [UnitOfWork]
    public override async Task DeleteAsync(Guid id)
    {
        var dbContext = (IHasEventOutbox)await DbContextProvider.GetDbContextAsync();
        var tableName = dbContext.OutgoingEvents.EntityType.GetSchemaQualifiedTableName();

        var sql = $"DELETE FROM \"{tableName}\" WHERE \"Id\" = '{id}'";
        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }
}
