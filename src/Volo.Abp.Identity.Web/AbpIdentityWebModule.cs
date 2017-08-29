using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.EmbeddedFiles;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.Navigation;

namespace Volo.Abp.Identity.Web
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule), typeof(AbpIdentityApplicationContractsModule))]
    public class AbpIdentityWebModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityWebModule>();

            services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
            });

            services.Configure<EmbeddedFileOptions>(options =>
            {
                options.Sources.Add(
                    new EmbeddedFileSet(
                        "/Areas/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.Identity.Web.Areas"
                        )
                    );
            });
        }
    }
}
