using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCacheTestDbContext : AbpDbContext<CompiledQueryCacheTestDbContext>
{
    public DbSet<CompiledQueryCacheTestRow> Rows { get; set; }

    public DbSet<CompiledQueryCacheSoftDeleteRow> SoftDeleteRows { get; set; }

    public DbSet<CompiledQueryCacheMultiTenantRow> MultiTenantRows { get; set; }

    public CompiledQueryCacheTestDbContext(DbContextOptions<CompiledQueryCacheTestDbContext> options)
        : base(options)
    {
    }
}
