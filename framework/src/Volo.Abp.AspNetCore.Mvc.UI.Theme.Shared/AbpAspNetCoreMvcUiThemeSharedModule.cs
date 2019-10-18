using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpAspNetCoreMvcUiPackagesModule),
        typeof(AbpAspNetCoreMvcUiWidgetsModule)
        )]
    public class AbpAspNetCoreMvcUiThemeSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAspNetCoreMvcUiThemeSharedModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiThemeSharedModule>("Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared");
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(StandardBundles.Styles.Global, bundle => { bundle.AddContributors(typeof(SharedThemeGlobalStyleContributor)); });

                options
                    .ScriptBundles
                    .Add(StandardBundles.Scripts.Global, bundle => bundle.AddContributors(typeof(SharedThemeGlobalScriptContributor)));
            });
        }
    }
}
