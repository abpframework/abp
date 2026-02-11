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
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Uow.EntityFrameworkCore;

public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : IEfCoreDbContext
{
    private const string TransactionsNotSupportedWarningMessage = "Current database does not support transactions. Your database may remain in an inconsistent state in an error case.";

    public ILogger<UnitOfWorkDbContextProvider<TDbContext>> Logger { get; set; }

    protected readonly IUnitOfWorkManager UnitOfWorkManager;
    protected readonly IConnectionStringResolver ConnectionStringResolver;
    protected readonly ICancellationTokenProvider CancellationTokenProvider;
    protected readonly ICurrentTenant CurrentTenant;
    protected readonly IEfCoreDbContextTypeProvider EfCoreDbContextTypeProvider;

    public UnitOfWorkDbContextProvider(
        IUnitOfWorkManager unitOfWorkManager,
        IConnectionStringResolver connectionStringResolver,
        ICancellationTokenProvider cancellationTokenProvider,
        ICurrentTenant currentTenant,
        IEfCoreDbContextTypeProvider efCoreDbContextTypeProvider)
    {
        UnitOfWorkManager = unitOfWorkManager;
        ConnectionStringResolver = connectionStringResolver;
        CancellationTokenProvider = cancellationTokenProvider;
        CurrentTenant = currentTenant;
        EfCoreDbContextTypeProvider = efCoreDbContextTypeProvider;

        Logger = NullLogger<UnitOfWorkDbContextProvider<TDbContext>>.Instance;
    }

    [Obsolete("Use GetDbContextAsync method.")]
    public virtual TDbContext GetDbContext()
    {
        if (UnitOfWork.EnableObsoleteDbContextCreationWarning &&
            !Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.Value)
        {
            Logger.LogWarning(
                "UnitOfWorkDbContextProvider.GetDbContext is deprecated. Use GetDbContextAsync instead! " +
                "You are probably using LINQ (LINQ extensions) directly on a repository. In this case, use repository.GetQueryableAsync() method " +
                "to obtain an IQueryable<T> instance and use LINQ (LINQ extensions) on this object. "
            );
            Logger.LogWarning(Environment.StackTrace.Truncate(2048));
        }

        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new AbpException("A DbContext can only be created inside a unit of work!");
        }

        var targetDbContextType = EfCoreDbContextTypeProvider.GetDbContextType(typeof(TDbContext));
        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
        var connectionString = ResolveConnectionString(connectionStringName);
        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";

        var databaseApi = unitOfWork.GetOrAddDatabaseApi(
            dbContextKey,
            () => new EfCoreDatabaseApi(
                CreateDbContext(unitOfWork, connectionStringName, connectionString)
            ));

        return (TDbContext)((EfCoreDatabaseApi)databaseApi).DbContext;
    }

    public virtual async Task<TDbContext> GetDbContextAsync()
    {
        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new AbpException("A DbContext can only be created inside a unit of work!");
        }

        var targetDbContextType = EfCoreDbContextTypeProvider.GetDbContextType(typeof(TDbContext));
        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
        var connectionString = await ResolveConnectionStringAsync(connectionStringName);

        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";

        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

        if (databaseApi == null)
        {
            databaseApi = new EfCoreDatabaseApi(
                await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString)
            );

            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TDbContext)((EfCoreDatabaseApi)databaseApi).DbContext;
    }

    [Obsolete("Use CreateDbContextAsync method.")]
    protected virtual TDbContext CreateDbContext(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
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

    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
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

    [Obsolete("Use CreateDbContextAsync.")]
    protected virtual TDbContext CreateDbContext(IUnitOfWork unitOfWork)
    {
        return unitOfWork.Options.IsTransactional
            ? CreateDbContextWithTransaction(unitOfWork)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
    }

    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
    {
        return unitOfWork.Options.IsTransactional
            ? await CreateDbContextWithTransactionAsync(unitOfWork)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
    }

    [Obsolete("Use CreateDbContextWithTransactionAsync.")]
    protected virtual TDbContext CreateDbContextWithTransaction(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

        if (activeTransaction == null)
        {
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            try
            {
                var dbtransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? dbContext.Database.BeginTransaction(unitOfWork.Options.IsolationLevel.Value)
                    : dbContext.Database.BeginTransaction();

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbtransaction,
                        dbContext,
                        CancellationTokenProvider
                    )
                );
            }
            catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
            {
                Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                return dbContext;
            }

            return dbContext;
        }
        else
        {
            DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext.As<DbContext>().HasRelationalTransactionManager())
            {
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    try
                    {
                        /* User did not re-use the ExistingConnection and we are starting a new transaction.
                            * EfCoreTransactionApi will check the connection string match and separately
                            * commit/rollback this transaction over the DbContext instance. */
                        if (unitOfWork.Options.IsolationLevel.HasValue)
                        {
                            dbContext.Database.BeginTransaction(unitOfWork.Options.IsolationLevel.Value);
                        }
                        else
                        {
                            dbContext.Database.BeginTransaction();
                        }
                    }
                    catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                    {
                        Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                        return dbContext;
                    }
                }
            }
            else
            {
                try
                {
                    /* No need to store the returning IDbContextTransaction for non-relational databases
                        * since EfCoreTransactionApi will handle the commit/rollback over the DbContext instance.
                        */
                    dbContext.Database.BeginTransaction();
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                    return dbContext;
                }
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);

            return dbContext;
        }
    }

    protected virtual async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

        if (activeTransaction == null)
        {
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            try
            {
                var dbTransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? await dbContext.Database.BeginTransactionAsync(unitOfWork.Options.IsolationLevel.Value, GetCancellationToken())
                    : await dbContext.Database.BeginTransactionAsync(GetCancellationToken());

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbTransaction,
                        dbContext,
                        CancellationTokenProvider
                    )
                );
            }
            catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
            {
                Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                return dbContext;
            }

            return dbContext;
        }
        else
        {
            DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext.As<DbContext>().HasRelationalTransactionManager())
            {
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction(), GetCancellationToken());
                }
                else
                {
                    try
                    {
                        /* User did not re-use the ExistingConnection and we are starting a new transaction.
                            * EfCoreTransactionApi will check the connection string match and separately
                            * commit/rollback this transaction over the DbContext instance. */
                        if (unitOfWork.Options.IsolationLevel.HasValue)
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                unitOfWork.Options.IsolationLevel.Value,
                                GetCancellationToken()
                            );
                        }
                        else
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                GetCancellationToken()
                            );
                        }
                    }
                    catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                    {
                        Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                        return dbContext;
                    }
                }
            }
            else
            {
                try
                {
                    /* No need to store the returning IDbContextTransaction for non-relational databases
                        * since EfCoreTransactionApi will handle the commit/rollback over the DbContext instance.
                          */
                    await dbContext.Database.BeginTransactionAsync(GetCancellationToken());
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                    return dbContext;
                }
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);

            return dbContext;
        }
    }

    protected virtual async Task<string> ResolveConnectionStringAsync(string connectionStringName)
    {
        // Multi-tenancy unaware contexts should always use the host connection string
        if (typeof(TDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
        {
            using (CurrentTenant.Change(null))
            {
                return await ConnectionStringResolver.ResolveAsync(connectionStringName);
            }
        }

        return await ConnectionStringResolver.ResolveAsync(connectionStringName);
    }

    [Obsolete("Use ResolveConnectionStringAsync method.")]
    protected virtual string ResolveConnectionString(string connectionStringName)
    {
        // Multi-tenancy unaware contexts should always use the host connection string
        if (typeof(TDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
        {
            using (CurrentTenant.Change(null))
            {
                return ConnectionStringResolver.Resolve(connectionStringName);
            }
        }

        return ConnectionStringResolver.Resolve(connectionStringName);
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return CancellationTokenProvider.FallbackToProvider(preferredValue);
    }
}
