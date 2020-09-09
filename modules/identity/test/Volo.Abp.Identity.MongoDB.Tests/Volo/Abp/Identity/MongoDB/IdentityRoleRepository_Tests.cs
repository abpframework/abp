using Xunit;

namespace Volo.Abp.Identity.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class IdentityRoleRepository_Tests : IdentityRoleRepository_Tests<AbpIdentityMongoDbTestModule>
    {

    }
}
