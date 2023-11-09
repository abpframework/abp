using MyCompanyName.MyProjectName.MongoDB;
using MyCompanyName.MyProjectName.Samples;
using Xunit;

namespace MyCompanyName.MyProjectName.MongoDb.Applications;

[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class MongoDBSampleAppServiceTests : SampleAppServiceTests<MyProjectNameMongoDbTestModule>
{

}
