using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapDemoModule),
        typeof(AspNetCoreTestBaseModule)
    )]
    public class AbpAspNetCoreMvcUiBootstrapDemoTestModule : AbpModule
    {

    }
}
