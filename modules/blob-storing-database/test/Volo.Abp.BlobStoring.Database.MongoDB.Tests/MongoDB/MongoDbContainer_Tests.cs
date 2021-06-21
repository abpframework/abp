using Xunit;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class MongoDbContainer_Tests : BlobContainer_Tests<BlobStoringDatabaseMongoDbTestModule>
    {
        
    }
}