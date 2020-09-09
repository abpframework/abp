using Xunit;

namespace Volo.Abp.IdentityServer
{
    [Collection(MongoTestCollection.Name)]
    public class ClientRepository_Tests : ClientRepository_Tests<AbpIdentityServerMongoDbTestModule>
    {

    }
}
