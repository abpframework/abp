using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(AbpUiNavigationModule))]
    public class AbpAspNetCoreMvcUiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreMvcUiModule>();

            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiModule>("Volo.Abp.AspNetCore.Mvc");
            });

            services.Configure<BundlingOptions>(options =>
            {
                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/libs/abp/abp.js?_v" + DateTime.Now.Ticks
                });
            });
        }
    }
}
