using Volo.CmsKit.Comments;
using Xunit;

namespace Volo.CmsKit.MongoDB.Comments
{
    [Collection(MongoTestCollection.Name)]
    public  class CommentRepository_Tests : CommentRepository_Tests<CmsKitMongoDbTestModule>
    {

    }
}
