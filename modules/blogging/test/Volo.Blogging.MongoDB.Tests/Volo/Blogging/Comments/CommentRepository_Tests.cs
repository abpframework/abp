using Volo.Blogging.Comments;
using Volo.Blogging.MongoDB;
using Xunit;

namespace Volo.Blogging
{
    [Collection(MongoTestCollection.Name)]
    public class CommentRepository_Tests : CommentRepository_Tests<BloggingMongoDbTestModule>
    {
    }
}