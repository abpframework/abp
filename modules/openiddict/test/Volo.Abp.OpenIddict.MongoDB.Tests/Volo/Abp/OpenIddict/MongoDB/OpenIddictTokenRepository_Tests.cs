using Xunit;

namespace Volo.Abp.OpenIddict.MongoDB;

[Collection(MongoTestCollection.Name)]
public class OpenIddictTokenRepository_Tests : OpenIddictTokenRepository_Tests<OpenIddictMongoDbTestModule>
{
    
}