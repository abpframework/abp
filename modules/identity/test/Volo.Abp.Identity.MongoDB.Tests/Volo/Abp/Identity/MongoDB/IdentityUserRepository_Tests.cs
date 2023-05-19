using Xunit;

namespace Volo.Abp.Identity.MongoDB;

[Collection(MongoTestCollection.Name)]
public class IdentityUserRepository_Tests : IdentityUserRepository_Tests<AbpIdentityMongoDbTestModule>
{

}
