using Microsoft.Extensions.DependencyInjection;
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
                throw new AbpException($"A {nameof(IMongoDatabase)} instance can only be created inside a unit of work!");
            }

            var connectionString = _connectionStringResolver.Resolve<TMongoDbContext>();
            var dbContextKey = $"{typeof(TMongoDbContext).FullName}_{connectionString}";

            string databaseName;
            if (connectionString.Contains("|"))
            {
                var splitted = connectionString.Split('|');
                connectionString = splitted[0];
                databaseName = splitted[1];
            }
            else
            {
                databaseName = ConnectionStringNameAttribute.GetConnStringName<TMongoDbContext>();
            }

            //TODO: Create only single MongoDbClient per connection string in an application (extract MongoClientCache for example).
            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                dbContextKey,
                () =>
                {
                    var database = new MongoClient(connectionString).GetDatabase(databaseName);

                    var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>();

                    var abpDbContext = unitOfWork.ServiceProvider.GetRequiredService<TMongoDbContext>().ToAbpMongoDbContext();
                    abpDbContext.InitializeDatabase(database);

                    return new MongoDbDatabaseApi<TMongoDbContext>(dbContext);
                });

            return ((MongoDbDatabaseApi<TMongoDbContext>)databaseApi).DbContext;
        }
    }
}