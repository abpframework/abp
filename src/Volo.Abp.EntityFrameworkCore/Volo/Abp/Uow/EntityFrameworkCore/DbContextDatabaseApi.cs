using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    public class DbContextDatabaseApi<TDbContext> : IDatabaseApi, ISupportsSavingChanges
        where TDbContext : AbpDbContext<TDbContext>
    {
        public TDbContext DbContext { get; }

        public DbContextDatabaseApi(TDbContext dbContext)
        {
            DbContext = dbContext;
        }
        
        public Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}