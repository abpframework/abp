using Volo.Abp.BlazoriseUI;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.UI.Theming.WebAssembly
{
    [DependsOn(
        typeof(AbpBlazoriseUIModule),
        typeof(AbpHttpClientIdentityModelWebAssemblyModule),
        typeof(AbpUiNavigationModule),
        typeof(AbpAspNetCoreComponentsUiThemingModule)
    )]
    public class AbpAspNetCoreComponentsWebAssemblyThemingModule : AbpModule
    {

    }
}
