using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions.Web.Localization.Resources.AbpPermissions;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Permissions.Web
{
    [DependsOn(typeof(AbpPermissionsApplicationContractsModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpPermissionsWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpPermissionsResource));
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionsWebModule>();

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPermissionsWebModule>("Volo.Abp.Permissions.Web");
            });
            
            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.AddVirtualJson<AbpPermissionsResource>("en", "/Localization/Resources/AbpPermissions");
            });

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpPermissionsWebAutoMapperProfile>(validate: true);
            });
        }
    }
}
