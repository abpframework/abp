using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Domain;

[Collection(MongoTestCollection.Name)]
public class EntityChange_Tests : EntityChange_Tests<AbpMongoDbTestModule>
{

}
