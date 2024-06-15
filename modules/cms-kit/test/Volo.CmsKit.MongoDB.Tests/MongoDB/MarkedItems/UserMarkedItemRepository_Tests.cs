using Volo.CmsKit.MarkedItems;
using Xunit;

namespace Volo.CmsKit.MongoDB.MarkedItems;

[Collection(MongoTestCollection.Name)]
public class UserMarkedItemRepository_Tests : UserMarkedItemRepository_Tests<CmsKitMongoDbTestModule>
{
}
