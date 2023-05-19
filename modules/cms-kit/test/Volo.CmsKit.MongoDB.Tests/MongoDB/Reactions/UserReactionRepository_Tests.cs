using Volo.CmsKit.Reactions;
using Xunit;

namespace Volo.CmsKit.MongoDB.Reactions;

[Collection(MongoTestCollection.Name)]
public class UserReactionRepository_Tests : UserReactionRepository_Tests<CmsKitMongoDbTestModule>
{
}
