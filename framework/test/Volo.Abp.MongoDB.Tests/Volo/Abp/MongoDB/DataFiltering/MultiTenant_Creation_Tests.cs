using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DataFiltering
{
    [Collection("MongoDB Collection")]
    public class MultiTenant_Creation_Tests : MultiTenant_Creation_Tests<AbpMongoDbTestModule>
    {
        
    }
}