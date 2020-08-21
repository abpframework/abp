using Xunit;

namespace Volo.Abp.Identity.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class IdentitySecurityLogRepository_Tests : IdentitySecurityLogRepository_Tests<AbpIdentityMongoDbTestModule>
    {

    }
}
