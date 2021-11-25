using Volo.Blogging.Posts;
using Volo.Blogging.MongoDB;
using Xunit;

namespace Volo.Blogging
{
    [Collection(MongoTestCollection.Name)]
    public class PostRepository_Tests : PostRepository_Tests<BloggingMongoDbTestModule>
    {
    }
}