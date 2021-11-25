using Xunit;

namespace Volo.Abp.FeatureManagement.MongoDB;

[Collection(MongoTestCollection.Name)]
public class FeatureValueRepositoryTests : FeatureValueRepository_Tests<AbpFeatureManagementMongoDbTestModule>
{

}
