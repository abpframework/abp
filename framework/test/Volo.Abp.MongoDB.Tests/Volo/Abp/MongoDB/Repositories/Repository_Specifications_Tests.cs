using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Repositories;

[Collection(MongoTestCollection.Name)]
public class Repository_Specifications_Tests : Repository_Specifications_Tests<AbpMongoDbTestModule>
{
}
