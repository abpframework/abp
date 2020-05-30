using Volo.Abp.BlobStoring.Database.MongoDB;
using Xunit;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    [Collection(MongoTestCollection.Name)]
    public class MongoDbContainer_Test : BlobContainer_Tests<BlobStoringDatabaseMongoDbTestModule>
    {
        
    }
}