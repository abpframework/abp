using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.Modularity;
using Volo.Abp.UI;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcClientCommonModule),
        typeof(AbpUiModule)
        )]
    public class AbpAspNetCoreComponentsWebAssemblyModule : AbpModule
    {

    }
}
