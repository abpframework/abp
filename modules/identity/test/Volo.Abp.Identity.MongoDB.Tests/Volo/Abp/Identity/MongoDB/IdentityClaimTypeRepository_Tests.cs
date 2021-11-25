using Xunit;

namespace Volo.Abp.Identity.MongoDB;

[Collection(MongoTestCollection.Name)]
public class IdentityClaimTypeRepository_Tests : IdentityClaimTypeRepository_Tests<AbpIdentityMongoDbTestModule>
{

}
