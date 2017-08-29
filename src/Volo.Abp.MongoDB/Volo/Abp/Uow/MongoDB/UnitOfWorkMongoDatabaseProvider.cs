using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Uow.MongoDB
{
    public class UnitOfWorkMongoDatabaseProvider<TMongoDbContext> : IMongoDatabaseProvider<TMongoDbContext>
        where TMongoDbContext : AbpMongoDbContext
    {
        public TMongoDbContext DbContext { get; }
        
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;

        public UnitOfWorkMongoDatabaseProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver,
            TMongoDbContext dbContext)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            DbContext = dbContext;
        }

        public IMongoDatabase GetDatabase()
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
                () => new MongoDbDatabaseApi(
                    new MongoClient(connectionString).GetDatabase(databaseName)
                ));

            return ((MongoDbDatabaseApi)databaseApi).Database;
        }
    }
}