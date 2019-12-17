using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DataFiltering
{
    [Collection("MongoDB Collection")]
    public class MultiTenant_Filter_Tests : MultiTenant_Filter_Tests<AbpMongoDbTestModule>
    {
        
    }
}