using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.PermissionManagement.Blazor;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Identity.Blazor
{
    [DependsOn(
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementBlazorModule),
        typeof(AbpBlazoriseUIModule)
        )]
    public class AbpIdentityBlazorModule : AbpModule
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpIdentityBlazorModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityBlazorAutoMapperProfile>(validate: true);
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
            });

            Configure<AbpRouterOptions>(options =>
            {
                options.AdditionalAssemblies.Add(typeof(AbpIdentityBlazorModule).Assembly);
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
}
