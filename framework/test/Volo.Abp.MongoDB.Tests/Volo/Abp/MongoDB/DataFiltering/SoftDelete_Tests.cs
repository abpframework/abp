using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DataFiltering
{
    [Collection("MongoDB Collection")]
    public class SoftDelete_Tests : SoftDelete_Tests<AbpMongoDbTestModule>
    {

    }
}
