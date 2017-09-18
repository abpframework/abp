using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.EmbeddedFiles;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.Navigation;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    public class AbpAspNetCoreMvcUiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreMvcUiModule>();

            services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new EmbeddedDemoMainMenuContributor());
            });

            services.Configure<EmbeddedFileOptions>(options =>
            {
                options.Sources.Add(
                    new EmbeddedFileSet(
                        "/Views/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.AspNetCore.Mvc.Views"
                        )
                    );

                options.Sources.Add(
                    new EmbeddedFileSet(
                        "/abp/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.AspNetCore.Mvc.wwwroot.abp"
                    )
                );
            });

            services.Configure<BundlingOptions>(options =>
            {
                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/abp/abp.js?_v" + DateTime.Now.Ticks
                });
            });
        }
    }
}
