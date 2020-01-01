using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TenantManagement.Web
{
    [DependsOn(typeof(AbpTenantManagementHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpFeatureManagementWebModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpTenantManagementWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpTenantManagementResource), typeof(AbpTenantManagementWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpTenantManagementWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpTenantManagementWebMainMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTenantManagementWebModule>("Volo.Abp.TenantManagement.Web");
            });

            context.Services.AddAutoMapperObjectMapper<AbpTenantManagementWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementWebAutoMapperProfile>(validate: true);
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/Index", TenantManagementPermissions.Tenants.Default);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/CreateModal", TenantManagementPermissions.Tenants.Create);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/EditModal", TenantManagementPermissions.Tenants.Update);
                options.Conventions.AuthorizePage("/TenantManagement/Tenants/ConnectionStrings", TenantManagementPermissions.Tenants.ManageConnectionStrings);
            });
        }
    }
}