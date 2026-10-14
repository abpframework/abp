using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCacheTestDbContext : AbpDbContext<CompiledQueryCacheTestDbContext>
{
    public DbSet<CompiledQueryCacheTestRow> Rows { get; set; }

    public CompiledQueryCacheTestDbContext(DbContextOptions<CompiledQueryCacheTestDbContext> options)
        : base(options)
    {
    }
}
