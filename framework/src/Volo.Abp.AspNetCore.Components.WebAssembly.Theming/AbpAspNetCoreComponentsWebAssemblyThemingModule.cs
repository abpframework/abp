using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebThemingModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyModule)
    )]
    public class AbpAspNetCoreComponentsWebAssemblyThemingModule : AbpModule
    {

    }
}
