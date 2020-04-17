using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpAspNetCoreMvcUiBundlingModule),
        typeof(AbpAutofacModule)
    )]
    public class AbpAspNetCoreMvcUiTestModule : AbpModule
    {

    }
}
