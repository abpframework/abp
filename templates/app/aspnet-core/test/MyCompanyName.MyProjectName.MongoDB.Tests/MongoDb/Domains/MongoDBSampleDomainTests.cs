using MyCompanyName.MyProjectName.Samples;
using Xunit;

namespace MyCompanyName.MyProjectName.MongoDB.Domains;

[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class MongoDBSampleDomainTests : SampleDomainTests<MyProjectNameMongoDbTestModule>
{

}
