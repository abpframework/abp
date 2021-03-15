using Microsoft.Extensions.DependencyInjection;
using MyCompanyName.MyProjectName.Web.Theme.Bundling;
using MyCompanyName.MyProjectName.Web.Theme.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName.Web.Theme
{
     [DependsOn(
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule)
        )]
    public class MyProjectNameWebThemeModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(MyProjectNameWebThemeModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpThemingOptions>(options =>
            {
                options.Themes.Add<MyProjectNameTheme>();

                if (options.DefaultThemeName == null)
                {
                    options.DefaultThemeName = MyProjectNameTheme.Name;
                }
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameWebThemeModule>("MyCompanyName.MyProjectName.Web.Theme");
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new MyProjectNameThemeMainTopToolbarContributor());
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(MyProjectNameThemeBundles.Styles.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Styles.Global)
                            .AddContributors(typeof(MyProjectNameThemeGlobalStyleContributor));
                    });

                options
                    .ScriptBundles
                    .Add(MyProjectNameThemeBundles.Scripts.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Scripts.Global)
                            .AddContributors(typeof(MyProjectNameThemeGlobalScriptContributor));
                    });
            });
        }
    }
}
