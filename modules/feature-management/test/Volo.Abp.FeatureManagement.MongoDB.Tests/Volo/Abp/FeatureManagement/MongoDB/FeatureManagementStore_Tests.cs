using Xunit;

namespace Volo.Abp.FeatureManagement.MongoDB;

[Collection(MongoTestCollection.Name)]
public class FeatureManagementStore_Tests : FeatureManagementStore_Tests<AbpFeatureManagementMongoDbTestModule>
{

}
