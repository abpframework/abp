using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DomainEvents
{
    [Collection("MongoDB Collection")]
    public class DomainEvents_Tests : DomainEvents_Tests<AbpMongoDbTestModule>
    {
    }
}