using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;
using Volo.Abp.FeatureManagement.Blazor.MudBlazor.Settings;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor.MudBlazor;

namespace Volo.Abp.FeatureManagement.Blazor.MudBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingMudBlazorModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpFeaturesModule),
    typeof(AbpSettingManagementBlazorMudBlazorModule)
)]
public class AbpFeatureManagementBlazorMudBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SettingManagementComponentOptions>(options =>
        {
            options.Contributors.Add(new FeatureSettingManagementComponentContributor());
        });
        
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpFeatureManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
