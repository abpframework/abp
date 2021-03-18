using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.Blazor.WebAssembly
{
    [DependsOn(
        typeof(AbpIdentityBlazorModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
    public class AbpIdentityBlazorWebAssemblyModule : AbpModule
    {
    }
}
