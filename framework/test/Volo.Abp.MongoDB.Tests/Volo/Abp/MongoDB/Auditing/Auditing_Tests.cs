using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Auditing
{
    [Collection(MongoTestCollection.Name)]
    public class Auditing_Tests : Auditing_Tests<AbpMongoDbTestModule>
    {

    }
}
