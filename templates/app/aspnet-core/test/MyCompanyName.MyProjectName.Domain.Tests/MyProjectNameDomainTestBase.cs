using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName;

/* Inherit from this class for your domain layer tests. */
public abstract class MyProjectNameDomainTestBase<TStartupModule> : MyProjectNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
