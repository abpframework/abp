using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpFeatureManagementDbProperties.ConnectionStringName)]
public interface IFeatureManagementDbContext : IEfCoreDbContext
{
    DbSet<FeatureValue> FeatureValues { get; }
}
