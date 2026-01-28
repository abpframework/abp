using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;

[DependsOn(
        typeof(AbpMudBlazorUIModule),
        typeof(AbpUiNavigationModule)
)]
public class AbpAspNetCoreComponentsWebThemingMudBlazorModule : AbpModule
{
}

