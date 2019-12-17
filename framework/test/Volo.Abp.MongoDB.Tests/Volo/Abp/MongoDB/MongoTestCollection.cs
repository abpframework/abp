using Xunit;

namespace Volo.Abp.MongoDB
{
    [CollectionDefinition("MongoDB Collection")]
    public class MongoTestCollection : ICollectionFixture<MongoDbFixture>
    {
        
    }
}