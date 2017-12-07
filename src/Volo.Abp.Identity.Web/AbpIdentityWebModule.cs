using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web.Areas.Identity.Localization.Resource;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.Identity.Web.ObjectMappings;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Identity.Web
{
    [DependsOn(typeof(AbpIdentityApplicationContractsModule))]
    [DependsOn(typeof(AbpAspNetCoreMvcUiBootstrapModule))]
    public class AbpIdentityWebModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityWebModule>();

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
                options.Resources.AddJson<IdentityResource>("en");
            });

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.Configurators.Add(context =>
                {
                    context.MapperConfiguration.AddProfile<AbpIdentityWebAutoMapperProfile>();
                });
            });
        }
    }
}
