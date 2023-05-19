using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Repositories;

[Collection(MongoTestCollection.Name)]
public class RepositoryExtensions_Tests : RepositoryExtensions_Tests<AbpMongoDbTestModule>
{

}
