using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    //TODO: Implement logic in DefaultDbContextResolver.Resolve in old ABP.

    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        public ILogger<UnitOfWorkDbContextProvider<TDbContext>> Logger { get; set; }

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _cancellationTokenProvider = cancellationTokenProvider;

            Logger = NullLogger<UnitOfWorkDbContextProvider<TDbContext>>.Instance;
        }

        [Obsolete("Use GetDbContextAsync method.")]
        public TDbContext GetDbContext()
        {
            if (!UnitOfWork.DisableObsoleteDbContextCreationWarning.Value)
            {
                Logger.LogWarning(
                    "UnitOfWorkDbContextProvider.GetDbContext is deprecated. Use GetDbContextAsync instead! " +
                    "You are probably using LINQ (LINQ extensions) directly on a repository. In this case, use repository.GetQueryableAsync() method " +
                    "to obtain an IQueryable<T> instance and use LINQ (LINQ extensions) on this object. "
                );
                Logger.LogWarning(Environment.StackTrace.Truncate(2048));
            }

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
                () => new EfCoreDatabaseApi<TDbContext>(
                    CreateDbContext(unitOfWork, connectionStringName, connectionString)
                ));

            return ((EfCoreDatabaseApi<TDbContext>)databaseApi).DbContext;
        }

        public async Task<TDbContext> GetDbContextAsync()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new AbpException("A DbContext can only be created inside a unit of work!");
            }

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = await _connectionStringResolver.ResolveAsync(connectionStringName);

            var dbContextKey = $"{typeof(TDbContext).FullName}_{connectionString}";

            var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

            if (databaseApi == null)
            {
                databaseApi = new EfCoreDatabaseApi<TDbContext>(
                    await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString)
                );

                unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
            }

            return ((EfCoreDatabaseApi<TDbContext>)databaseApi).DbContext;
        }

        private TDbContext CreateDbContext(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
        {
            var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
            using (DbContextCreationContext.Use(creationContext))
            {
                var dbContext = CreateDbContext(unitOfWork);

                if (dbContext is IAbpEfCoreDbContext abpEfCoreDbContext)
                {
                    abpEfCoreDbContext.Initialize(
                        new AbpEfCoreDbContextInitializationContext(
                            unitOfWork
                        )
                    );
                }

                return dbContext;
            }
        }

        private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
        {
            var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
            using (DbContextCreationContext.Use(creationContext))
            {
                var dbContext = await CreateDbContextAsync(unitOfWork);

                if (dbContext is IAbpEfCoreDbContext abpEfCoreDbContext)
                {
                    abpEfCoreDbContext.Initialize(
                        new AbpEfCoreDbContextInitializationContext(
                            unitOfWork
                        )
                    );
                }

                return dbContext;
            }
        }

        private TDbContext CreateDbContext(IUnitOfWork unitOfWork)
        {
            return unitOfWork.Options.IsTransactional
                ? CreateDbContextWithTransaction(unitOfWork)
                : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
        }

        private async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
        {
            return unitOfWork.Options.IsTransactional
                ? await CreateDbContextWithTransactionAsync(unitOfWork)
                : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
        }

        private TDbContext CreateDbContextWithTransaction(IUnitOfWork unitOfWork)
        {
            var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
            var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

            if (activeTransaction == null)
            {
                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

                var dbtransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? dbContext.Database.BeginTransaction(unitOfWork.Options.IsolationLevel.Value)
                    : dbContext.Database.BeginTransaction();

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbtransaction,
                        dbContext
                    )
                );

                return dbContext;
            }
            else
            {
                DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

                if (dbContext.As<DbContext>().HasRelationalTransactionManager())
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction(); //TODO: Why not using the new created transaction?
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);

                return dbContext;
            }
        }

        private async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
        {
            var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
            var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

            if (activeTransaction == null)
            {
                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

                var dbTransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? await dbContext.Database.BeginTransactionAsync(unitOfWork.Options.IsolationLevel.Value, GetCancellationToken())
                    : await dbContext.Database.BeginTransactionAsync(GetCancellationToken());

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbTransaction,
                        dbContext
                    )
                );

                return dbContext;
            }
            else
            {
                DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

                if (dbContext.As<DbContext>().HasRelationalTransactionManager())
                {
                    await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction(), GetCancellationToken());
                }
                else
                {
                    await dbContext.Database.BeginTransactionAsync(GetCancellationToken()); //TODO: Why not using the new created transaction?
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);

                return dbContext;
            }
        }

        protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
        {
            return _cancellationTokenProvider.FallbackToProvider(preferredValue);
        }
    }
}
