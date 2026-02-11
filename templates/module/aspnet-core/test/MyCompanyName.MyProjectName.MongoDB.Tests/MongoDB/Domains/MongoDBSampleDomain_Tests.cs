using MyCompanyName.MyProjectName.Samples;
using Xunit;

namespace MyCompanyName.MyProjectName.MongoDB.Domains;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleDomain_Tests : SampleManager_Tests<MyProjectNameMongoDbTestModule>
{

}
