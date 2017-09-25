using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.Uow.EntityFrameworkCore
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

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = _connectionStringResolver.Resolve(connectionStringName);

            var dbContextKey = $"{typeof(TDbContext).FullName}_{connectionString}";

            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                dbContextKey,
                () => new DbContextDatabaseApi<TDbContext>(
                    CreateDbContext(unitOfWork, connectionStringName, connectionString)
                ));

            return ((DbContextDatabaseApi<TDbContext>)databaseApi).DbContext;
        }

        private static TDbContext CreateDbContext(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
        {
            var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
            using (DbContextCreationContext.Use(creationContext))
            {
                return unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
            }
        }
    }
}