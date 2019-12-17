using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Domain
{
    [Collection("MongoDB Collection")]
    public class ConcurrencyStamp_Tests : ConcurrencyStamp_Tests<AbpMongoDbTestModule>
    {

    }
}
