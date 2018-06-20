using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TenantManagement.Web
{
    [DependsOn(typeof(AbpTenantManagementHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpTenantManagementWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpTenantManagementResource), typeof(AbpTenantManagementWebModule).Assembly);
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpTenantManagementWebMainMenuContributor());
            });

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTenantManagementWebModule>("Volo.Abp.TenantManagement.Web");
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpTenantManagementResource>()
                    .AddBaseTypes(
                        typeof(AbpValidationResource),
                        typeof(AbpUiResource)
                    ).AddVirtualJson("/Localization/Resources/AbpTenantManagement/Web");
            });

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementWebAutoMapperProfile>(validate: true);
            });

            services.Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/Index", TenantManagementPermissions.Tenants.Default);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/CreateModal", TenantManagementPermissions.Tenants.Create);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/EditModal", TenantManagementPermissions.Tenants.Update);
            });

            services.AddAssemblyOf<AbpTenantManagementWebModule>();
        }
    }
}