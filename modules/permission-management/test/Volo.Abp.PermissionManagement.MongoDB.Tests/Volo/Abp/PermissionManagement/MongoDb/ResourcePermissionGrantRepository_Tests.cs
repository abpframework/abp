using Xunit;

namespace Volo.Abp.PermissionManagement.MongoDB;

[Collection(MongoTestCollection.Name)]
public class ResourcePermissionGrantRepository_Tests : ResourcePermissionGrantRepository_Tests<AbpPermissionManagementMongoDbTestModule>
{

}
