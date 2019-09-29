using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories.MemoryDb;
using Volo.Abp.MemoryDb;

namespace Volo.Abp.Uow.MemoryDb
{
    public class UnitOfWorkMemoryDatabaseProvider<TMemoryDbContext> : IMemoryDatabaseProvider<TMemoryDbContext>
        where TMemoryDbContext : MemoryDbContext
    {
        public TMemoryDbContext DbContext { get; }
        
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;
        private readonly MemoryDatabaseManager _memoryDatabaseManager;

        public UnitOfWorkMemoryDatabaseProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver,
            TMemoryDbContext dbContext, 
            MemoryDatabaseManager memoryDatabaseManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            DbContext = dbContext;
            _memoryDatabaseManager = memoryDatabaseManager;
        }

        public IMemoryDatabase GetDatabase()
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new AbpException($"A {nameof(IMemoryDatabase)} instance can only be created inside a unit of work!");
            }

            var connectionString = _connectionStringResolver.Resolve<TMemoryDbContext>();
            var dbContextKey = $"{typeof(TMemoryDbContext).FullName}_{connectionString}";

            var databaseApi = unitOfWork.GetOrAddDatabaseApi(
                dbContextKey,
                () => new MemoryDbDatabaseApi(
                    _memoryDatabaseManager.Get(connectionString)
                ));

            return ((MemoryDbDatabaseApi)databaseApi).Database;
        }
    }
}