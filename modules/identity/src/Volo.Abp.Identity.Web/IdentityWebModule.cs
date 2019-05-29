﻿using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Identity.Web
{
    [DependsOn(typeof(IdentityHttpApiModule))]
    [DependsOn(typeof(AspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AutoMapperModule))]
    [DependsOn(typeof(PermissionManagementWebModule))]
    [DependsOn(typeof(AspNetCoreMvcUiThemeSharedModule))]
    public class IdentityWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(IdentityResource), typeof(IdentityWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
            });

            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<IdentityWebModule>("Volo.Abp.Identity.Web");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<IdentityResource>()
                    .AddBaseTypes(
                        typeof(AbpValidationResource),
                        typeof(AbpUiResource)
                    ).AddVirtualJson("/Localization/Resources/AbpIdentity");
            });

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

            Configure<BundlingOptions>(options =>
            {
                options
                    .ScriptBundles
                    .Get(StandardBundles.Scripts.Global)
                    .AddFiles("/Pages/Identity/Shared/change-password-modal.js")
                    .AddFiles("/Pages/Identity/Shared/personal-settings-modal.js");
            });
        }
    }
}
