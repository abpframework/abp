using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Xunit;

namespace MyCompanyName.MyProjectName;

[CollectionDefinition(MyProjectNameTestConsts.CollectionDefinitionName)]
public class MyProjectNameApplicationCollection : MyProjectNameEntityFrameworkCoreCollectionFixtureBase
{

}
