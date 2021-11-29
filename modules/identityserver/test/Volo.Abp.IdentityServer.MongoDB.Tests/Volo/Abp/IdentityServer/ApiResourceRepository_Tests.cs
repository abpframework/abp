using Xunit;

namespace Volo.Abp.IdentityServer;

[Collection(MongoTestCollection.Name)]
public class ApiResourceRepository_Tests : ApiResourceRepository_Tests<AbpIdentityServerMongoDbTestModule>
{
}
