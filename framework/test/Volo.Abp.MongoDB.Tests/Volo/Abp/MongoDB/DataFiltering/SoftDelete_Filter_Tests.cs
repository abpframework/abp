using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DataFiltering
{
    [Collection(MongoTestCollection.Name)]
    public class SoftDelete_Filter_Tests : SoftDelete_Filter_Tests<AbpMongoDbTestModule>
    {
        
    }
}