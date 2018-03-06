using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web.Localization.Resources.AbpIdentity;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions.Web;
using Volo.Abp.Ui.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Identity.Web
{
    [DependsOn(typeof(AbpIdentityHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpPermissionsWebModule))]
    public class AbpIdentityWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(IdentityResource));
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
            });

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpIdentityWebModule>("Volo.Abp.Identity.Web");
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.AddVirtualJson<IdentityResource>("en", "/Localization/Resources/AbpIdentity");
            });

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityWebAutoMapperProfile>(validate: true);
            });

            services.Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Identity/Users/Index", IdentityPermissions.Users.Default);
                options.Conventions.AuthorizePage("/Identity/Users/CreateModal", IdentityPermissions.Users.Create);
                options.Conventions.AuthorizePage("/Identity/Users/EditModal", IdentityPermissions.Users.Update);
                options.Conventions.AuthorizePage("/Identity/Roles/Index", IdentityPermissions.Roles.Default);
                options.Conventions.AuthorizePage("/Identity/Roles/CreateModal", IdentityPermissions.Roles.Create);
                options.Conventions.AuthorizePage("/Identity/Roles/EditModal", IdentityPermissions.Roles.Update);
            });

            services.AddAssemblyOf<AbpIdentityWebModule>();
        }
    }
}
