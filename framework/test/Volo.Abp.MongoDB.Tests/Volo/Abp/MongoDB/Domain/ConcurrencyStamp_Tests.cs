using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Domain;

[Collection(MongoTestCollection.Name)]
public class ConcurrencyStamp_Tests : ConcurrencyStamp_Tests<AbpMongoDbTestModule>
{

}
