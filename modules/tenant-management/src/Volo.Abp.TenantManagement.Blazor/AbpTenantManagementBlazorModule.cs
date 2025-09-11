using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.Mapperly;
using Volo.Abp.FeatureManagement.Blazor;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.TenantManagement.Blazor;

[DependsOn(
    typeof(AbpMapperlyModule),
    typeof(AbpTenantManagementApplicationContractsModule),
    typeof(AbpFeatureManagementBlazorModule)
)]
public class AbpTenantManagementBlazorModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpTenantManagementBlazorModule>();

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new TenantManagementBlazorMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpTenantManagementBlazorModule).Assembly);
        });
        
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpTenantManagementResource>()
                .AddBaseTypes(
                    typeof(AbpFeatureManagementResource),
                    typeof(AbpUiResource));
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    TenantManagementModuleExtensionConsts.ModuleName,
                    TenantManagementModuleExtensionConsts.EntityNames.Tenant,
                    createFormTypes: new[] { typeof(TenantCreateDto) },
                    editFormTypes: new[] { typeof(TenantUpdateDto) }
                );
        });
    }
}
