using Volo.Blogging.Tagging;
using Volo.Blogging.MongoDB;
using Xunit;

namespace Volo.Blogging
{
    [Collection(MongoTestCollection.Name)]
    public class TagRepository_Tests : TagRepository_Tests<BloggingMongoDbTestModule>
    {
    }
}