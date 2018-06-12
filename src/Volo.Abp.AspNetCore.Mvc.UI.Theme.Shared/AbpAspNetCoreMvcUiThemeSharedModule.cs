using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpAspNetCoreMvcUiPackagesModule)
        )]
    public class AbpAspNetCoreMvcUiThemeSharedModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiThemeSharedModule>("Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared");
            });

            services.Configure<BundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(StandardBundles.Styles.Global)
                    .AddContributors(typeof(SharedThemeGlobalStyleContributor));

                options
                    .ScriptBundles
                    .Add(StandardBundles.Scripts.Global)
                    .AddContributors(typeof(SharedThemeGlobalScriptContributor));
            });

            services.AddAssemblyOf<AbpAspNetCoreMvcUiThemeSharedModule>();
        }
    }
}
