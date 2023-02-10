using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.TestApp.FifthContext;

public class FifthDbContext : AbpDbContext<FifthDbContext>, IFifthDbContext
{
    public DbSet<FifthDbContextDummyEntity> FifthDbContextDummyEntity { get; set; }

    public DbSet<FifthDbContextMultiTenantDummyEntity> FifthDbContextMultiTenantDummyEntity { get; set; }

    public FifthDbContext(DbContextOptions<FifthDbContext> options)
        : base(options)
    {
    }
}
