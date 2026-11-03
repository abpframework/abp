using Volo.Blogging.MongoDB;
using Volo.Blogging.Users;
using Xunit;

namespace Volo.Blogging
{
    [Collection(MongoTestCollection.Name)]
    public class BlogUserRepository_Tests : BlogUserRepository_Tests<BloggingMongoDbTestModule>
    {
    }
}
