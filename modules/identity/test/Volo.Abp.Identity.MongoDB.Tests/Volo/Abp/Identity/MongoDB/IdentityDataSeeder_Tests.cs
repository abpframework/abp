using Xunit;

namespace Volo.Abp.Identity.MongoDB;

[Collection(MongoTestCollection.Name)]
public class IdentityDataSeeder_Tests : IdentityDataSeeder_Tests<AbpIdentityMongoDbTestModule>
{

}
