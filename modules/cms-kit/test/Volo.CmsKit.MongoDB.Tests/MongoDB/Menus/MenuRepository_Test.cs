using Xunit;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.MongoDB.Menus
{
    [Collection(MongoTestCollection.Name)]
    public class MenuRepository_Test : MenuRepository_Test<CmsKitMongoDbTestModule>
    {
        
    }
}