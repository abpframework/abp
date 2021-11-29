using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DomainEvents
{
    [Collection(MongoTestCollection.Name)]
    public class EntityChangeEvents_Tests : EntityChangeEvents_Tests<AbpMongoDbTestModule>
    {

    }
}
