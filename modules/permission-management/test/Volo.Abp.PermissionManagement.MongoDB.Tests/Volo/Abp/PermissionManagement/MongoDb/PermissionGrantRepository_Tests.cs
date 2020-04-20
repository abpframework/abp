using Xunit;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class PermissionGrantRepository_Tests : PermissionGrantRepository_Tests<AbpPermissionManagementMongoDbTestModule>
    {

    }
}
