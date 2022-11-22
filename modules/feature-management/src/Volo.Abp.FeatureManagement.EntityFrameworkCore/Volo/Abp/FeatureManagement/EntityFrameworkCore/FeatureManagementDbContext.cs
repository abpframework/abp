using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpFeatureManagementDbProperties.ConnectionStringName)]
public class FeatureManagementDbContext : AbpDbContext<FeatureManagementDbContext>, IFeatureManagementDbContext
{
    public DbSet<FeatureGroupDefinitionRecord> FeatureGroups { get; set; }

    public DbSet<FeatureDefinitionRecord> Features { get; set; }

    public DbSet<FeatureValue> FeatureValues { get; set; }

    public FeatureManagementDbContext(DbContextOptions<FeatureManagementDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureFeatureManagement();
    }
}
