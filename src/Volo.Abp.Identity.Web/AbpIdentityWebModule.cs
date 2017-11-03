using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Identity.Web.Areas.Identity.Localization.Resource;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.VirtualFileSystem.Embedded;

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
                options.FileSets.Add(
                    new EmbeddedFileSet(
                        "/Pages/",
                        GetType().Assembly,
                        "Volo.Abp.Identity.Web.Pages"
                    )
                );

                options.FileSets.Add(
                    new EmbeddedFileSet(
                        "/Areas/",
                        GetType().Assembly,
                        "Volo.Abp.Identity.Web.Areas"
                    )
                );

                options.FileSets.Add(
                    new EmbeddedFileSet(
                        "/",
                        GetType().Assembly,
                        "Volo.Abp.Identity.Web.wwwroot"
                    )
                );
            });

            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.AddJson<IdentityResource>("en");
            });
        }
    }
}
