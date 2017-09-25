using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    //TODO: Implement logic in DefaultDbContextResolver.Resolve in old ABP.

    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : AbpDbContext<TDbContext>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;
        private readonly IEfCoreTransactionStrategy _transactionStrategy;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver,
            IEfCoreTransactionStrategy transactionStrategy)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _transactionStrategy = transactionStrategy;
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

        private TDbContext CreateDbContext(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
        {
            var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
            using (DbContextCreationContext.Use(creationContext))
            {
                TDbContext dbContext;

                if (unitOfWork.Options.IsTransactional == true)
                {
                    dbContext = _transactionStrategy.CreateDbContext<TDbContext>(unitOfWork, creationContext);
                }
                else
                {
                    dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
                }

                if (unitOfWork.Options.Timeout.HasValue &&
                    dbContext.Database.IsRelational() &&
                    !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    dbContext.Database.SetCommandTimeout(unitOfWork.Options.Timeout.Value.TotalSeconds.To<int>());
                }

                return dbContext;
            }
        }
    }
}