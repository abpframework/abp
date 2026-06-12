using Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;
using Volo.Abp.AspNetCore.Components.MauiBlazor;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsMauiBlazorThemingMudBlazorBundlingModule),
    typeof(AbpAspNetCoreComponentsWebThemingMudBlazorModule),
    typeof(AbpAspNetCoreComponentsMauiBlazorModule)
)]
public class AbpAspNetCoreComponentsMauiBlazorThemingMudBlazorModule : AbpModule
{
}
