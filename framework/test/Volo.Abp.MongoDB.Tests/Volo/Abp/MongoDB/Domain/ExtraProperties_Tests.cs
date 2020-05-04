using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Domain
{
    [Collection(MongoTestCollection.Name)]
    public class ExtraProperties_Tests : ExtraProperties_Tests<AbpMongoDbTestModule>
    {

    }
}
