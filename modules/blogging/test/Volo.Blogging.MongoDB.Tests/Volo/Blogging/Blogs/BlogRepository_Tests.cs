using Volo.Blogging.Blogs;
using Volo.Blogging.MongoDB;
using Xunit;

namespace Volo.Blogging
{
    [Collection(MongoTestCollection.Name)]
    public class BlogRepository_Tests : BlogRepository_Tests<BloggingMongoDbTestModule>
    {
    }
}