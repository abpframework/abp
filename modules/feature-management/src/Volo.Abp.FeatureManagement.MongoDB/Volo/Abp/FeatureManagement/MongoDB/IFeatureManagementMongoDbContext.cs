using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement.MongoDB;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpFeatureManagementDbProperties.ConnectionStringName)]
public interface IFeatureManagementMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<FeatureGroupDefinitionRecord> FeatureGroups { get; }

    IMongoCollection<FeatureDefinitionRecord> Features { get; }

    IMongoCollection<FeatureValue> FeatureValues { get; }
}
