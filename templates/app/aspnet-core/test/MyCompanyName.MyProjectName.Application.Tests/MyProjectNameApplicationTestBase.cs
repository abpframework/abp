using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName;

public abstract class MyProjectNameApplicationTestBase<TStartupModule> : MyProjectNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
