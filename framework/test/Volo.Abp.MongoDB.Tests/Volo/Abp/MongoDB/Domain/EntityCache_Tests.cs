using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Domain;

[Collection(MongoTestCollection.Name)]
public class EntityCache_Tests : EntityCache_Tests<AbpMongoDbTestModule>
{
}