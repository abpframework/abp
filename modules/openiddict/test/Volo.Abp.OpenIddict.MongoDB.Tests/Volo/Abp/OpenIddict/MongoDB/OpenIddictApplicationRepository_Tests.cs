using Xunit;

namespace Volo.Abp.OpenIddict.MongoDB;

[Collection(MongoTestCollection.Name)]
public class OpenIddictApplicationRepository_Tests : OpenIddictApplicationRepository_Tests<OpenIddictMongoDbTestModule>
{
    
}