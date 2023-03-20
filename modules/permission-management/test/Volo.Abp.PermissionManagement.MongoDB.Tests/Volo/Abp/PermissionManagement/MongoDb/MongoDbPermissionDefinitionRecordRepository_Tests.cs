using Xunit;

namespace Volo.Abp.PermissionManagement.MongoDB;

[Collection(MongoTestCollection.Name)]
public class MongoDbPermissionDefinitionRecordRepository_Tests : PermissionGrantRepository_Tests<AbpPermissionManagementMongoDbTestModule>
{

}
