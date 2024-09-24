using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Favicon;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.HighlightJs;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Prismjs;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Demo.Menus;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.LayoutHooks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAutofacModule)
    )]
public class AbpAspNetCoreMvcUiBootstrapDemoModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.MapAbpStaticAssets();
        app.UseConfiguredEndpoints();
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles
                .Get(StandardBundles.Styles.Global)
                .AddFiles("/css/demo.css")
                .AddContributors(typeof(PrismjsStyleBundleContributor))
                .AddContributors(typeof(HighlightJsStyleContributor));
            
            options.ScriptBundles
                .Get(StandardBundles.Scripts.Global)
                .AddFiles("/js/demo.js")
                .AddContributors(typeof(PrismjsScriptBundleContributor))
                .AddContributors(typeof(HighlightJsScriptContributor));
        } );
        
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new BootstrapDemoMenuContributor());
        });
        
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                StandardBundles.Styles.Global, 
                bundleConfiguration =>
                {
                    bundleConfiguration.AddFiles("/styles/my-global-styles.css");
                }
            );
        });

        Configure<AbpLayoutHookOptions>(options =>
        {
            options.Add(LayoutHooks.Head.First, typeof(FaviconViewComponent));
        });
    }
}
