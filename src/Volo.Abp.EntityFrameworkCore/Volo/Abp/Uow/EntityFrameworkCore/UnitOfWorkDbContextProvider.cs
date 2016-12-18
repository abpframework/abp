using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

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

            var connectionString = _connectionStringResolver.Resolve<TDbContext>();
            var dbContextKey = $"{typeof(TDbContext).FullName}_{connectionString}";

            //TODO: It would be very good if we could pass the connection string to DbContext options while creating DbContext! Because _connectionStringResolver.Resolve is called twice in current implementation.
            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                dbContextKey,
                () => new DbContextDatabaseApi<TDbContext>(
                    unitOfWork.ServiceProvider.GetRequiredService<TDbContext>()
                ));
            
            return ((DbContextDatabaseApi<TDbContext>)databaseApi).DbContext;
        }
    }
}