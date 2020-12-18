using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity.Web
{
    [DependsOn(typeof(AbpIdentityHttpApiModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpPermissionManagementWebModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiThemeSharedModule))]
    public class AbpIdentityWebModule : AbpModule
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(IdentityResource), typeof(AbpIdentityWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpIdentityWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpIdentityWebModule>();
            });

            context.Services.AddAutoMapperObjectMapper<AbpIdentityWebModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityWebAutoMapperProfile>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Identity/Users/Index", IdentityPermissions.Users.Default);
                options.Conventions.AuthorizePage("/Identity/Users/CreateModal", IdentityPermissions.Users.Create);
                options.Conventions.AuthorizePage("/Identity/Users/EditModal", IdentityPermissions.Users.Update);
                options.Conventions.AuthorizePage("/Identity/Roles/Index", IdentityPermissions.Roles.Default);
                options.Conventions.AuthorizePage("/Identity/Roles/CreateModal", IdentityPermissions.Roles.Create);
                options.Conventions.AuthorizePage("/Identity/Roles/EditModal", IdentityPermissions.Roles.Update);
            });
            
            
            Configure<AbpPageToolbarOptions>(options =>
            {
                options.Configure<Volo.Abp.Identity.Web.Pages.Identity.Users.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<IdentityResource>("NewUser"),
                            icon: "plus",
                            name: "CreateUser",
                            requiredPolicyName: IdentityPermissions.Users.Create
                        );
                    }
                );
                
                options.Configure<Volo.Abp.Identity.Web.Pages.Identity.Roles.IndexModel>(
                    toolbar =>
                    {
                        toolbar.AddButton(
                            LocalizableString.Create<IdentityResource>("NewRole"),
                            icon: "plus",
                            name: "CreateRole",
                            requiredPolicyName: IdentityPermissions.Roles.Create
                        );
                    }
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
                        createFormTypes: new[] { typeof(Volo.Abp.Identity.Web.Pages.Identity.Roles.CreateModalModel.RoleInfoModel) },
                        editFormTypes: new[] { typeof(Volo.Abp.Identity.Web.Pages.Identity.Roles.EditModalModel.RoleInfoModel) }
                    );
                
                ModuleExtensionConfigurationHelper
                    .ApplyEntityConfigurationToUi(
                        IdentityModuleExtensionConsts.ModuleName,
                        IdentityModuleExtensionConsts.EntityNames.User,
                        createFormTypes: new[] { typeof(Volo.Abp.Identity.Web.Pages.Identity.Users.CreateModalModel.UserInfoViewModel) },
                        editFormTypes: new[] { typeof(Volo.Abp.Identity.Web.Pages.Identity.Users.EditModalModel.UserInfoViewModel) }
                    );
            });
        }
    }
}
