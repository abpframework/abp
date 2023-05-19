using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.TestApp.FifthContext;

namespace Volo.Abp.TestApp.EntityFrameworkCore;

public class HostTestAppDbContext : AbpDbContext<HostTestAppDbContext>, IFifthDbContext
{
    public DbSet<FifthDbContextDummyEntity> FifthDbContextDummyEntity { get; set; }
    public DbSet<FifthDbContextMultiTenantDummyEntity> FifthDbContextMultiTenantDummyEntity { get; set; }

    public HostTestAppDbContext(DbContextOptions<HostTestAppDbContext> options)
        : base(options)
    {
    }
}
