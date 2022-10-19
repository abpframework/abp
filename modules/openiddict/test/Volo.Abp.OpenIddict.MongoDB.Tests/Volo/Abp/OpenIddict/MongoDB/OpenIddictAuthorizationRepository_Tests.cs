using Xunit;

namespace Volo.Abp.OpenIddict.MongoDB;

[Collection(MongoTestCollection.Name)]
public class OpenIddictAuthorizationRepository_Tests : OpenIddictAuthorizationRepository_Tests<OpenIddictMongoDbTestModule>
{
    
}