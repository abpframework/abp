using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.TestApp.FifthContext;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TestApp.EntityFrameworkCore;

[ReplaceDbContext(typeof(IFifthDbContext), MultiTenancySides.Tenant)]
public class TenantTestAppDbContext : AbpDbContext<TenantTestAppDbContext>, IFifthDbContext
{
    public DbSet<FifthDbContextDummyEntity> FifthDbContextDummyEntity { get; set; }
    public DbSet<FifthDbContextMultiTenantDummyEntity> FifthDbContextMultiTenantDummyEntity { get; set; }

    public TenantTestAppDbContext(DbContextOptions<TenantTestAppDbContext> options)
        : base(options)
    {
    }
}
