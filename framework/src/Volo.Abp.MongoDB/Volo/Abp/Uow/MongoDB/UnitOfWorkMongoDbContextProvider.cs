using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Uow.MongoDB
{
    public class UnitOfWorkMongoDbContextProvider<TMongoDbContext> : IMongoDbContextProvider<TMongoDbContext>
        where TMongoDbContext : IAbpMongoDbContext
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;

        public UnitOfWorkMongoDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
        }

        public TMongoDbContext GetDbContext()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new AbpException(
                    $"A {nameof(IMongoDatabase)} instance can only be created inside a unit of work!");
            }

            var connectionString = _connectionStringResolver.Resolve<TMongoDbContext>();
            var dbContextKey = $"{typeof(TMongoDbContext).FullName}_{connectionString}";

            var mongoUrl = new MongoUrl(connectionString);
            var databaseName = mongoUrl.DatabaseName;
            if (databaseName.IsNullOrWhiteSpace())
            {
                databaseName = ConnectionStringNameAttribute.GetConnStringName<TMongoDbContext>();
            }

            //TODO: Create only single MongoDbClient per connection string in an application (extract MongoClientCache for example).
            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                dbContextKey,
                () => new MongoDbDatabaseApi<TMongoDbContext>(CreateDbContext(unitOfWork, mongoUrl, databaseName)));

            return ((MongoDbDatabaseApi<TMongoDbContext>) databaseApi).DbContext;
        }

        private TMongoDbContext CreateDbContext(IUnitOfWork unitOfWork, MongoUrl mongoUrl, string databaseName)
        {
            var client = new MongoClient(mongoUrl);
            var database = client.GetDatabase(databaseName);

            if (unitOfWork.Options.IsTransactional)
            {
                return CreateDbContextWithTransaction(unitOfWork, mongoUrl, client, database);
            }

            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>();
            dbContext.ToAbpMongoDbContext().InitializeDatabase(database, null);

            return dbContext;
        }

        public TMongoDbContext CreateDbContextWithTransaction(
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

                session.StartTransaction();

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new MongoDbTransactionApi(session)
                );

                dbContext.ToAbpMongoDbContext().InitializeDatabase(database, session);
            }
            else
            {
                dbContext.ToAbpMongoDbContext().InitializeDatabase(database, activeTransaction.SessionHandle);
            }

            return dbContext;
        }
    }
}
