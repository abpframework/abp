using Xunit;

namespace Volo.Abp.Identity.MongoDB;

[Collection(MongoTestCollection.Name)]
public class IdentityUserManager_SharedUser_Tests : IdentityUserManager_SharedUser_Tests<AbpIdentityMongoDbTestModule>
{
}
