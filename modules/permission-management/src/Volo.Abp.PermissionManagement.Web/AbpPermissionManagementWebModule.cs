using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web.Localization.Resources.AbpPermissionManagement;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.PermissionManagement.Web
{
    [DependsOn(typeof(AbpPermissionManagementApplicationContractsModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpPermissionManagementWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpPermissionManagementResource));
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionManagementWebModule>();

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPermissionManagementWebModule>("Volo.Abp.PermissionManagement.Web");
            });
            
            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AbpPermissionManagementResource>("en")
                    .AddVirtualJson("/Localization/Resources/AbpPermissionManagement");
            });

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpPermissionManagementWebAutoMapperProfile>(validate: true);
            });
        }
    }
}
