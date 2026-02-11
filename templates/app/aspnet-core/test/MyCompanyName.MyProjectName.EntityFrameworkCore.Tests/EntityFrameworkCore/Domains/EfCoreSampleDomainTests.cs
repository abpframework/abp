using MyCompanyName.MyProjectName.Samples;
using Xunit;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore.Domains;

[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MyProjectNameEntityFrameworkCoreTestModule>
{

}
