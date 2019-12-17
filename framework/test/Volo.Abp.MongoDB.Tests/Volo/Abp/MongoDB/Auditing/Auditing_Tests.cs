using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Auditing
{
    [Collection("MongoDB Collection")]
    public class Auditing_Tests : Auditing_Tests<AbpMongoDbTestModule>
    {

    }
}
