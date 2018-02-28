using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy.Web.Localization.Resources.AbpMultiTenancy;
using Volo.Abp.MultiTenancy.Web.Navigation;
using Volo.Abp.Ui.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.MultiTenancy.Web
{
    [DependsOn(typeof(AbpMultiTenancyHttpApiModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpMultiTenancyWebModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpMultiTenancyResource));
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpMultiTenancyWebModule>();

            services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpMultiTenancyWebMainMenuContributor());
            });

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpMultiTenancyWebModule>("Volo.Abp.MultiTenancy.Web");
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.AddVirtualJson<AbpMultiTenancyResource>("en", "/Localization/Resources/AbpMultiTenancy");
            });

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpMultiTenancyWebAutoMapperProfile>(validate: true);
            });
        }
    }
}