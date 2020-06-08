using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Repositories
{
    [Collection(MongoTestCollection.Name)]
    public class Repository_Queryable_Tests : Repository_Queryable_Tests<AbpMongoDbTestModule>
    {
        
    }
}