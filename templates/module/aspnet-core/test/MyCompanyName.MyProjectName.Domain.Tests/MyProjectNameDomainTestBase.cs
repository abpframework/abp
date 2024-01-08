using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class MyProjectNameDomainTestBase<TStartupModule> : MyProjectNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
