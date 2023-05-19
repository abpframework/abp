using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MemoryDb;

namespace Volo.Abp.MemoryDb;

public interface IMemoryDatabaseProvider<TMemoryDbContext>
    where TMemoryDbContext : MemoryDbContext
{
    [Obsolete("Use GetDbContextAsync method.")]
    TMemoryDbContext DbContext { get; }

    Task<TMemoryDbContext> GetDbContextAsync();

    [Obsolete("Use GetDatabaseAsync method.")]
    IMemoryDatabase GetDatabase();

    Task<IMemoryDatabase> GetDatabaseAsync();
}
