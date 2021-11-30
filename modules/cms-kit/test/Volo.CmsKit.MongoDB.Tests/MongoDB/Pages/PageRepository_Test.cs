using Volo.CmsKit.Pages;
using Xunit;

namespace Volo.CmsKit.MongoDB.Pages;

[Collection(MongoTestCollection.Name)]
public class PageRepository_Test : PageRepository_Test<CmsKitMongoDbTestModule>
{

}
