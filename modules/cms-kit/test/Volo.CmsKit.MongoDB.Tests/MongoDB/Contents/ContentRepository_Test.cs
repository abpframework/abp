using Volo.CmsKit.Contents;
using Xunit;

namespace Volo.CmsKit.MongoDB.Contents
{
    [Collection(MongoTestCollection.Name)]
    public class ContentRepository_Test : ContentRepository_Tests<CmsKitMongoDbTestModule>
    {
        
    }
}