using Volo.CmsKit.Tags;
using Xunit;

namespace Volo.CmsKit.MongoDB.Tags
{
    [Collection(MongoTestCollection.Name)]
    public class TagRepository_Test : TagRepository_Test<CmsKitMongoDbTestModule>
    {
        
    }
}