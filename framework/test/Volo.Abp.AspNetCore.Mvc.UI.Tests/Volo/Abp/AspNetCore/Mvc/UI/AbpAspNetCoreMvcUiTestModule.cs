using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI
{
    [DependsOn(
        typeof(AspNetCoreTestBaseModule),
        typeof(AspNetCoreMvcUiBundlingModule),
        typeof(AutofacModule)
    )]
    public class AbpAspNetCoreMvcUiTestModule : AbpModule
    {

    }
}
