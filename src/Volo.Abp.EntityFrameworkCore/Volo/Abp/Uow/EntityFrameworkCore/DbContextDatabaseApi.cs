using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    public class DbContextDatabaseApi<TDbContext> : IDatabaseApi
        where TDbContext : AbpDbContext<TDbContext>
    {
        public TDbContext DbContext { get; }

        public DbContextDatabaseApi(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public Task CommitAsync()
        {
            return DbContext.SaveChangesAsync();
        }
    }
}