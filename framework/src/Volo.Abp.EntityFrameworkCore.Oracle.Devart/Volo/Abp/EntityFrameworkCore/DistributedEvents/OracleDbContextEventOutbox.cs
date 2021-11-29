using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class OracleDbContextEventOutbox<TDbContext> : DbContextEventOutbox<TDbContext> , IOracleDbContextEventOutbox<TDbContext>
        where TDbContext : IHasEventOutbox
    {
        public OracleDbContextEventOutbox(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        [UnitOfWork]
        public override async Task DeleteAsync(Guid id)
        {
            var dbContext = (IHasEventOutbox) await DbContextProvider.GetDbContextAsync();
            var tableName = dbContext.OutgoingEvents.EntityType.GetSchemaQualifiedTableName();

            var sql = $"DELETE FROM \"{tableName}\" WHERE \"Id\" = HEXTORAW('{GuidToOracleType(id)}')";
            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }

        protected virtual string GuidToOracleType(Guid id)
        {
            return BitConverter.ToString(id.ToByteArray()).Replace("-", "").ToUpper();
        }
    }
}
