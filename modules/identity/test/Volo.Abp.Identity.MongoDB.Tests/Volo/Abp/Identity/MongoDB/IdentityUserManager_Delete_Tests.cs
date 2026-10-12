using Xunit;

namespace Volo.Abp.Identity.MongoDB;

[Collection(MongoTestCollection.Name)]
public class IdentityUserManager_Delete_Tests : IdentityUserManager_Delete_Tests<AbpIdentityMongoDbTestModule>
{
}
