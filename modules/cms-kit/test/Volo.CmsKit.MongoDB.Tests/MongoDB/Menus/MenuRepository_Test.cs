using Xunit;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.MongoDB.Menus;

[Collection(MongoTestCollection.Name)]
public class MenuRepository_Test : MenuItemRepository_Test<CmsKitMongoDbTestModule>
{

}
