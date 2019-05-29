using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared
{
    [DependsOn(
        typeof(AspNetCoreMvcUiBootstrapModule),
        typeof(AspNetCoreMvcUiPackagesModule)
        )]
    public class AspNetCoreMvcUiThemeSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AspNetCoreMvcUiThemeSharedModule>("Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared");
            });

            Configure<BundlingOptions>(options =>
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
