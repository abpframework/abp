using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule)
        )]
    public class AbpAspNetCoreMvcUiBasicThemeModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<ThemingOptions>(options =>
            {
                options.Themes.Add<BasicTheme>();

                if (options.DefaultThemeName == null)
                {
                    options.DefaultThemeName = BasicTheme.Name;
                }
            });

            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiBasicThemeModule>("Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic");
            });

            Configure<ToolbarOptions>(options =>
            {
                options.Contributors.Add(new BasicThemeMainTopToolbarContributor());
            });

            Configure<BundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(BasicThemeBundles.Styles.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Styles.Global)
                            .AddContributors(typeof(BasicThemeGlobalStyleContributor));
                    });

                options
                    .ScriptBundles
                    .Add(BasicThemeBundles.Scripts.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Scripts.Global)
                            .AddContributors(typeof(BasicThemeGlobalScriptContributor));
                    });
            });
        }
    }
}
