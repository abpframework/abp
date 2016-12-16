using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.Repositories.EntityFrameworkCore
{
    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : AbpDbContext<TDbContext>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
        }

        public TDbContext GetDbContext()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new AbpException("A DbContext can only be created inside a unit of work!");
            }

            var moduleName = "";//TODO: Get module name from DbContext?
            var dbContextKey = $"{moduleName}_{typeof(TDbContext).FullName}_{_connectionStringResolver.Resolve(moduleName)}";

            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                dbContextKey,
                () => new DbContextDatabaseApi<TDbContext>(
                    unitOfWork.ServiceProvider.GetRequiredService<TDbContext>()
                ));
            
            return ((DbContextDatabaseApi<TDbContext>)databaseApi).DbContext;
        }
    }

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