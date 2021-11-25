using Xunit;

namespace Volo.Abp.IdentityServer;

[Collection(MongoTestCollection.Name)]
public class PersistentGrantRepository_Tests : PersistentGrantRepository_Tests<AbpIdentityServerMongoDbTestModule>
{

}
