using Volo.Abp.Domain.Repositories.MemoryDb;

namespace Volo.Abp.MemoryDb
{
    public interface IMemoryDatabaseProvider<TMemoryDbContext>
        where TMemoryDbContext : MemoryDbContext
    {
        TMemoryDbContext DbContext { get; }

        IMemoryDatabase GetDatabase();
    }
}