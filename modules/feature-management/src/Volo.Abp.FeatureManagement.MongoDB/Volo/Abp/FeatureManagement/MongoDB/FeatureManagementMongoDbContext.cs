using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement.MongoDB;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpFeatureManagementDbProperties.ConnectionStringName)]
public class FeatureManagementMongoDbContext : AbpMongoDbContext, IFeatureManagementMongoDbContext
{
    public IMongoCollection<FeatureGroupDefinitionRecord> FeatureGroups => Collection<FeatureGroupDefinitionRecord>();
    public IMongoCollection<FeatureDefinitionRecord> Features => Collection<FeatureDefinitionRecord>();
    public IMongoCollection<FeatureValue> FeatureValues => Collection<FeatureValue>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureFeatureManagement();
    }
}
