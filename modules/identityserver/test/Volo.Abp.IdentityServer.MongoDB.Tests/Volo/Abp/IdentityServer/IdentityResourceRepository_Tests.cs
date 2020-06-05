using Xunit;

namespace Volo.Abp.IdentityServer
{
    [Collection(MongoTestCollection.Name)]
    public class IdentityResourceRepository_Tests : IdentityResourceRepository_Tests<AbpIdentityServerMongoDbTestModule>
    {
    }
}
