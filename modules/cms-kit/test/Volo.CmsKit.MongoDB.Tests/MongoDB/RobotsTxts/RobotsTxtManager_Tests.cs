using Volo.CmsKit.RobotsTxts;
using Xunit;

namespace Volo.CmsKit.MongoDB.RobotsTxts;

[Collection(MongoTestCollection.Name)]
public class RobotsTxtManager_Tests : RobotsTxtManager_Test<CmsKitMongoDbTestModule>
{
}
