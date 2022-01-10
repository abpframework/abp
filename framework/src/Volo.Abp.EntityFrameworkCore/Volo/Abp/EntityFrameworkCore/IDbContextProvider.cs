using System;
using System.Threading.Tasks;

namespace Volo.Abp.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
    where TDbContext : IEfCoreDbContext
{
    [Obsolete("Use GetDbContextAsync method.")]
    TDbContext GetDbContext();

    Task<TDbContext> GetDbContextAsync();
}
