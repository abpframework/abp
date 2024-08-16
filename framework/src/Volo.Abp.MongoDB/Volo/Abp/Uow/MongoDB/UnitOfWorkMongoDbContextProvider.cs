using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Uow.MongoDB;

public class UnitOfWorkMongoDbContextProvider<TMongoDbContext> : IMongoDbContextProvider<TMongoDbContext>
    where TMongoDbContext : IAbpMongoDbContext
{
    private const string TransactionsNotSupportedWarningMessage = "Current database does not support transactions. Your database may remain in an inconsistent state in an error case.";
    public ILogger<UnitOfWorkMongoDbContextProvider<TMongoDbContext>> Logger { get; set; }

    protected readonly IUnitOfWorkManager UnitOfWorkManager;
    protected readonly IConnectionStringResolver ConnectionStringResolver;
    protected readonly ICancellationTokenProvider CancellationTokenProvider;
    protected readonly ICurrentTenant CurrentTenant;
    protected readonly AbpMongoDbContextOptions Options;
    protected readonly IMongoDbContextTypeProvider DbContextTypeProvider;

    public UnitOfWorkMongoDbContextProvider(
        IUnitOfWorkManager unitOfWorkManager,
        IConnectionStringResolver connectionStringResolver,
        ICancellationTokenProvider cancellationTokenProvider,
        ICurrentTenant currentTenant,
        IOptions<AbpMongoDbContextOptions> options,
        IMongoDbContextTypeProvider dbContextTypeProvider)
    {
        UnitOfWorkManager = unitOfWorkManager;
        ConnectionStringResolver = connectionStringResolver;
        CancellationTokenProvider = cancellationTokenProvider;
        CurrentTenant = currentTenant;
        DbContextTypeProvider = dbContextTypeProvider;
        Options = options.Value;

        Logger = NullLogger<UnitOfWorkMongoDbContextProvider<TMongoDbContext>>.Instance;
    }

    [Obsolete("Use CreateDbContextAsync")]
    public virtual TMongoDbContext GetDbContext()
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
            throw new AbpException(
                $"A {nameof(IMongoDatabase)} instance can only be created inside a unit of work!");
        }

        var targetDbContextType = DbContextTypeProvider.GetDbContextType(typeof(TMongoDbContext));
        var connectionString = ResolveConnectionString(targetDbContextType);
        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";

        var mongoUrl = new MongoUrl(connectionString);
        var databaseName = mongoUrl.DatabaseName;
        if (databaseName.IsNullOrWhiteSpace())
        {
            databaseName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
        }

        //TODO: Create only single MongoDbClient per connection string in an application (extract MongoClientCache for example).
        var databaseApi = unitOfWork.GetOrAddDatabaseApi(
            dbContextKey,
            () => new MongoDbDatabaseApi(CreateDbContext(unitOfWork, mongoUrl, databaseName)));

        return (TMongoDbContext)((MongoDbDatabaseApi) databaseApi).DbContext;
    }

    public virtual async Task<TMongoDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new AbpException(
                $"A {nameof(IMongoDatabase)} instance can only be created inside a unit of work!");
        }

        var targetDbContextType = DbContextTypeProvider.GetDbContextType(typeof(TMongoDbContext));
        var connectionString = await ResolveConnectionStringAsync(targetDbContextType);
        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";

        var mongoUrl = new MongoUrl(connectionString);
        var databaseName = mongoUrl.DatabaseName;
        if (databaseName.IsNullOrWhiteSpace())
        {
            databaseName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType);
        }

        //TODO: Create only single MongoDbClient per connection string in an application (extract MongoClientCache for example).
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);
        if (databaseApi == null)
        {
            databaseApi = new MongoDbDatabaseApi(
                await CreateDbContextAsync(
                    unitOfWork,
                    mongoUrl,
                    databaseName,
                    cancellationToken
                )
            );

            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TMongoDbContext)((MongoDbDatabaseApi) databaseApi).DbContext;
    }

    [Obsolete("Use CreateDbContextAsync")]

    private TMongoDbContext CreateDbContext(IUnitOfWork unitOfWork, MongoUrl mongoUrl, string databaseName)
    {
        var client = CreateMongoClient(mongoUrl);
        var database = client.GetDatabase(databaseName);

        if (unitOfWork.Options.IsTransactional)
        {
            return CreateDbContextWithTransaction(unitOfWork, mongoUrl, client, database);
        }

        var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>();
        dbContext.ToAbpMongoDbContext().InitializeDatabase(database, client, null);

        return dbContext;
    }

    protected virtual async Task<TMongoDbContext> CreateDbContextAsync(
        IUnitOfWork unitOfWork,
        MongoUrl mongoUrl,
        string databaseName,
        CancellationToken cancellationToken = default)
    {
        var client = CreateMongoClient(mongoUrl);
        var database = client.GetDatabase(databaseName);

        if (unitOfWork.Options.IsTransactional)
        {
            return await CreateDbContextWithTransactionAsync(
                unitOfWork,
                mongoUrl,
                client,
                database,
                cancellationToken
            );
        }

        var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>();
        dbContext.ToAbpMongoDbContext().InitializeDatabase(database, client, null);

        return dbContext;
    }

    [Obsolete("Use CreateDbContextWithTransactionAsync")]
    protected virtual TMongoDbContext CreateDbContextWithTransaction(
        IUnitOfWork unitOfWork,
        MongoUrl url,
        MongoClient client,
        IMongoDatabase database)
    {
        var transactionApiKey = $"MongoDb_{url}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as MongoDbTransactionApi;
        var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>();

        if (activeTransaction?.SessionHandle == null)
        {
            var session = client.StartSession();

            if (unitOfWork.Options.Timeout.HasValue)
            {
                session.AdvanceOperationTime(new BsonTimestamp(unitOfWork.Options.Timeout.Value));
            }

            try
            {
                session.StartTransaction();
            }
            catch (NotSupportedException e)
            {
                Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                dbContext.ToAbpMongoDbContext().InitializeDatabase(database, client, null);
                return dbContext;
            }

            unitOfWork.AddTransactionApi(
                transactionApiKey,
                new MongoDbTransactionApi(
                    session,
                    CancellationTokenProvider
                )
            );

            dbContext.ToAbpMongoDbContext().InitializeDatabase(database, client, session);
        }
        else
        {
            dbContext.ToAbpMongoDbContext().InitializeDatabase(database, client, activeTransaction.SessionHandle);
        }

        return dbContext;
    }

    protected virtual async Task<TMongoDbContext> CreateDbContextWithTransactionAsync(
        IUnitOfWork unitOfWork,
        MongoUrl url,
        MongoClient client,
        IMongoDatabase database,
        CancellationToken cancellationToken = default)
    {
        var transactionApiKey = $"MongoDb_{url}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as MongoDbTransactionApi;
        var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>();

        if (activeTransaction?.SessionHandle == null)
        {
            var session = await client.StartSessionAsync(cancellationToken: GetCancellationToken(cancellationToken));

            if (unitOfWork.Options.Timeout.HasValue)
            {
                session.AdvanceOperationTime(new BsonTimestamp(unitOfWork.Options.Timeout.Value));
            }

            try
            {
                session.StartTransaction();
            }
            catch (NotSupportedException)
            {
                Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                dbContext.ToAbpMongoDbContext().InitializeDatabase(database, client, null);
                return dbContext;
            }

            unitOfWork.AddTransactionApi(
                transactionApiKey,
                new MongoDbTransactionApi(
                    session,
                    CancellationTokenProvider
                )
            );

            dbContext.ToAbpMongoDbContext().InitializeDatabase(database, client, session);
        }
        else
        {
            dbContext.ToAbpMongoDbContext().InitializeDatabase(database, client, activeTransaction.SessionHandle);
        }

        return dbContext;
    }

    protected virtual async Task<string> ResolveConnectionStringAsync(Type dbContextType)
    {
        // Multi-tenancy unaware contexts should always use the host connection string
        if (typeof(TMongoDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
        {
            using (CurrentTenant.Change(null))
            {
                return await ConnectionStringResolver.ResolveAsync(dbContextType);
            }
        }

        return await ConnectionStringResolver.ResolveAsync(dbContextType);
    }

    [Obsolete("Use ResolveConnectionStringAsync method.")]
    protected virtual string ResolveConnectionString(Type dbContextType)
    {
        // Multi-tenancy unaware contexts should always use the host connection string
        if (typeof(TMongoDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
        {
            using (CurrentTenant.Change(null))
            {
                return ConnectionStringResolver.Resolve(dbContextType);
            }
        }

        return ConnectionStringResolver.Resolve(dbContextType);
    }

    protected virtual MongoClient CreateMongoClient(MongoUrl mongoUrl)
    {
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
        Options.MongoClientSettingsConfigurer?.Invoke(mongoClientSettings);

        return new MongoClient(mongoClientSettings);
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return CancellationTokenProvider.FallbackToProvider(preferredValue);
    }
}
