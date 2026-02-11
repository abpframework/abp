using MyCompanyName.MyProjectName.Samples;
using Xunit;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore.Applications;

[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MyProjectNameEntityFrameworkCoreTestModule>
{

}
