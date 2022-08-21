using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.FeatureManagement.Blazor.Settings;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor;

namespace Volo.Abp.FeatureManagement.Blazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpFeaturesModule),
    typeof(AbpSettingManagementBlazorModule)
)]
public class AbpFeatureManagementBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SettingManagementComponentOptions>(options =>
        {
            options.Contributors.Add(new FeatureSettingManagementComponentContributor());
        });
    }
}
