using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public class SqlRawDbContextEventOutbox<TDbContext> : DbContextEventOutbox<TDbContext>, ISqlRawDbContextEventOutbox<TDbContext>
    where TDbContext : IHasEventOutbox
{
    public SqlRawDbContextEventOutbox(IDbContextProvider<TDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    [UnitOfWork]
    public override async Task DeleteAsync(Guid id)
    {
        var dbContext = (IHasEventOutbox)await DbContextProvider.GetDbContextAsync();
        var tableName = dbContext.OutgoingEvents.EntityType.GetSchemaQualifiedTableName();

        var sql = $"DELETE FROM {tableName} WHERE Id = '{id.ToString().ToUpper()}'";
        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }
}
