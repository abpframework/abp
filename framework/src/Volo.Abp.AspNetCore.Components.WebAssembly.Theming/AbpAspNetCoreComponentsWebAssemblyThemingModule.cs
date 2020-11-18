using Volo.Abp.BlazoriseUI;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming
{
    [DependsOn(
        typeof(AbpBlazoriseUIModule),
        typeof(AbpHttpClientIdentityModelWebAssemblyModule),
        typeof(AbpUiNavigationModule)
    )]
    public class AbpAspNetCoreComponentsWebAssemblyThemingModule : AbpModule
    {

    }
}
