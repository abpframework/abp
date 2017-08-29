using Volo.Abp.Domain.Repositories.MemoryDb;

namespace Volo.Abp.Uow.MemoryDb
{
    public class MemoryDbDatabaseApi: IDatabaseApi
    {
        public IMemoryDatabase Database { get; }

        public MemoryDbDatabaseApi(IMemoryDatabase database)
        {
            Database = database;
        }
    }
}
