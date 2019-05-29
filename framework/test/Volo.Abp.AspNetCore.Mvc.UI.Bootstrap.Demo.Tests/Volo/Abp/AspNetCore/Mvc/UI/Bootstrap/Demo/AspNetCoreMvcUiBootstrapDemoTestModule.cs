using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo
{
    [DependsOn(
        typeof(AspNetCoreMvcUiBootstrapDemoModule),
        typeof(AspNetCoreTestBaseModule)
    )]
    public class AspNetCoreMvcUiBootstrapDemoTestModule : AbpModule
    {

    }
}
