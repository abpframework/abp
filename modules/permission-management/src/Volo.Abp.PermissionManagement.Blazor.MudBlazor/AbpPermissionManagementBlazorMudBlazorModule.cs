using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Localization;

namespace Volo.Abp.PermissionManagement.Blazor.MudBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingMudBlazorModule),
    typeof(AbpPermissionManagementApplicationContractsModule)
    )]
public class AbpPermissionManagementBlazorMudBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpPermissionManagementResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
