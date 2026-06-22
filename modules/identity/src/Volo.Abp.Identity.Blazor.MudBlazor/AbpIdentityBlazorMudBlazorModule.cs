using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Mapperly;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.PermissionManagement.Blazor.MudBlazor;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Identity.Blazor.MudBlazor;

[DependsOn(
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpMapperlyModule),
    typeof(AbpPermissionManagementBlazorMudBlazorModule),
    typeof(AbpMudBlazorUIModule)
    )]
public class AbpIdentityBlazorMudBlazorModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpIdentityBlazorMudBlazorModule>();

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpIdentityBlazorMudBlazorModule).Assembly);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IdentityResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.Role,
                    createFormTypes: new[] { typeof(IdentityRoleCreateDto) },
                    editFormTypes: new[] { typeof(IdentityRoleUpdateDto) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    createFormTypes: new[] { typeof(IdentityUserCreateDto) },
                    editFormTypes: new[] { typeof(IdentityUserUpdateDto) }
                );
        });
    }
}
