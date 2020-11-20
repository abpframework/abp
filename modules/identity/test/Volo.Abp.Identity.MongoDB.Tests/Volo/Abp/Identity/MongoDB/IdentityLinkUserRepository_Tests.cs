using Xunit;

namespace Volo.Abp.Identity.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class IdentityLinkUserRepository_Tests : IdentityLinkUserRepository_Tests<AbpIdentityMongoDbTestModule>
    {

    }
}
