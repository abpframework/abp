using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;

[DependsOn(
        typeof(AbpAspNetCoreComponentsWebModule),
        typeof(AbpMudBlazorUIModule),
        typeof(AbpUiNavigationModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpGlobalFeaturesModule),
        typeof(AbpFeaturesModule)
)]
public class AbpAspNetCoreComponentsWebThemingMudBlazorModule : AbpModule
{
}

