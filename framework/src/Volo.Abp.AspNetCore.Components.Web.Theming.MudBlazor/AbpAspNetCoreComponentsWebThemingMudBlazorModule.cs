using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.MudBlazorUI;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpMudBlazorUIModule)
)]
public class AbpAspNetCoreComponentsWebThemingMudBlazorModule : AbpModule
{
}

