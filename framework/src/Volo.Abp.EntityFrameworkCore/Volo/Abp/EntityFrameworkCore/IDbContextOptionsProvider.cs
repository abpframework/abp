using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore
{
    public interface IDbContextOptionsProvider<TDbContext>
        where TDbContext : AbpDbContext<TDbContext>
    {
        [Obsolete("Use GetDbContextAsync method.")]
        DbContextOptions<TDbContext> GetDbContextOptions();

        Task<DbContextOptions<TDbContext>> GetDbContextOptionsAsync();
    }
}
