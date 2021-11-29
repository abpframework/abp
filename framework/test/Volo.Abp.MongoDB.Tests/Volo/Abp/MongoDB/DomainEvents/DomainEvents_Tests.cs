using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DomainEvents;

[Collection(MongoTestCollection.Name)]
public class DomainEvents_Tests : DomainEvents_Tests<AbpMongoDbTestModule>
{
}
