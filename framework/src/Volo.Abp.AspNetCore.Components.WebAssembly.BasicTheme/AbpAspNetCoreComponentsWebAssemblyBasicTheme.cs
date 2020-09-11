using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
        )]
    public class AbpAspNetCoreComponentsWebAssemblyBasicThemeModule : AbpModule
    {

    }
}
