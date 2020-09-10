using Volo.Abp.BlazoriseUI;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme
{
    [DependsOn(
        typeof(AbpBlazoriseUIModule),
        typeof(AbpHttpClientIdentityModelWebAssemblyModule)
        )]
    public class AbpAspNetCoreComponentsWebAssemblyBasicThemeModule : AbpModule
    {

    }
}
