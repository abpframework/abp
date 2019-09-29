using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore
{
    [ConnectionStringName(FeatureManagementConsts.ConnectionStringName)]
    public interface IFeatureManagementDbContext : IEfCoreDbContext
    {
        DbSet<FeatureValue> FeatureValues { get; set; }
    }
}