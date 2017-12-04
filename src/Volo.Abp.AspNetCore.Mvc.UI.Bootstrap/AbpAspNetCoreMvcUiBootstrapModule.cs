using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiModule))]
    public class AbpAspNetCoreMvcUiBootstrapModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreMvcUiBootstrapModule>();
            
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiBootstrapModule>("Volo.Abp.AspNetCore.Mvc.UI.Bootstrap");
            });

            services.Configure<BundlingOptions>(options =>
            {
                options.StyleBundles.Add("GlobalStyles", new[]
                {
                    "/libs/material/css/material.min.css",
                    "/libs/datatables/datatables.css",
                    "/styles/libs/datatables.css"
                });

                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/libs/jquery/jquery-3.1.1.min.js",
                    "/libs/bootstrap/js/bootstrap.bundle.min.js",
                    "/libs/material/js/material.min.js",
                    "/libs/datatables/datatables.min.js",
                    "/libs/vue/vue.js",
                    "/libs/abp/abp-jquery.js?_v" + DateTime.Now.Ticks
                });
            });
        }
    }
}
