using MyCompanyName.MyProjectName.MongoDB;
using MyCompanyName.MyProjectName.Samples;
using Xunit;

namespace MyCompanyName.MyProjectName.MongoDb.Applications;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleAppService_Tests : SampleAppService_Tests<MyProjectNameMongoDbTestModule>
{

}
