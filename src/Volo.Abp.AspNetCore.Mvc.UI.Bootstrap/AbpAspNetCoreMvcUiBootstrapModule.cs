using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.EmbeddedFiles;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiModule))]
    public class AbpAspNetCoreMvcUiBootstrapModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreMvcUiBootstrapModule>();
            
            services.Configure<EmbeddedFileOptions>(options =>
            {
                options.Sources.Add(
                    new EmbeddedFileSet(
                        "/libs/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.libs"
                        )
                    );

                options.Sources.Add(
                    new EmbeddedFileSet(
                        "/Views/",
                        GetType().GetTypeInfo().Assembly,
                        "Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Views"
                        )
                    );
            });

            services.Configure<BundlingOptions>(options =>
            {
                options.StyleBundles.Add("GlobalStyles", new[]
                {
                    "/libs/bootstrap/css/bootstrap.min.css"
                });

                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/libs/jquery/jquery-3.1.1.min.js",
                    "/libs/tether/js/tether.min.js",
                    "/libs/bootstrap/js/bootstrap.min.js",
                    "/abp/abp.jquery.js?_v" + DateTime.Now.Ticks
                });
            });
        }
    }
}
