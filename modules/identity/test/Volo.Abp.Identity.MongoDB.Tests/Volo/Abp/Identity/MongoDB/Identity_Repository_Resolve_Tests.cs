using Xunit;

namespace Volo.Abp.Identity.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class Identity_Repository_Resolve_Tests : Identity_Repository_Resolve_Tests<AbpIdentityMongoDbTestModule>
    {
    }
}
