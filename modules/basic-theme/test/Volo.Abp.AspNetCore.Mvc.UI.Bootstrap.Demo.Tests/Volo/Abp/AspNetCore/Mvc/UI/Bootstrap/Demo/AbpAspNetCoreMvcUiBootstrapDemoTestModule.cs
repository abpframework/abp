using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBootstrapDemoModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class AbpAspNetCoreMvcUiBootstrapDemoTestModule : AbpModule
{

}
