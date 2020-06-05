using Xunit;

namespace Volo.Abp.TenantManagement.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class TenantRepository_Tests : TenantRepository_Tests<AbpTenantManagementMongoDbTestModule>
    {

    }
}
