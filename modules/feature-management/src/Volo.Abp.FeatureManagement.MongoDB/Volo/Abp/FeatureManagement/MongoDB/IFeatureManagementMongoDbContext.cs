using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(FeatureManagementDbProperties.ConnectionStringName)]
    public interface IFeatureManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<FeatureValue> FeatureValues { get; }
    }
}
