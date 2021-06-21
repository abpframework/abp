using Xunit;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    [CollectionDefinition(MyProjectNameTestConsts.CollectionDefinitionName)]
    public class MyProjectNameEntityFrameworkCoreCollection : ICollectionFixture<MyProjectNameEntityFrameworkCoreFixture>
    {

    }
}
