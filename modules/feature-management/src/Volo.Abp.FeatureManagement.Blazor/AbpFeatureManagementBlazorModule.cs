using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.Blazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpFeaturesModule)
)]
public class AbpFeatureManagementBlazorModule : AbpModule
{

}
