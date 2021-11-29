using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DataFiltering
{
    [Collection(MongoTestCollection.Name)]
    public class MultiTenant_Filter_Tests : MultiTenant_Filter_Tests<AbpMongoDbTestModule>
    {
        
    }
}