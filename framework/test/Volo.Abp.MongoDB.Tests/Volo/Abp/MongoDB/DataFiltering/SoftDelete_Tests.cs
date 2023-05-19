using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DataFiltering;

[Collection(MongoTestCollection.Name)]
public class SoftDelete_Tests : SoftDelete_Tests<AbpMongoDbTestModule>
{

}
