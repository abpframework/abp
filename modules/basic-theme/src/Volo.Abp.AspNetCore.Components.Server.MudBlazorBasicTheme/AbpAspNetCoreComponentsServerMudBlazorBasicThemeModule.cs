using Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme.Bundling;
using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor;
using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor.Bundling;
using Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using BlazorStandardBundles = Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor.Bundling.BlazorServerMudBlazorStandardBundles;

namespace Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebMudBlazorBasicThemeModule),
    typeof(AbpAspNetCoreComponentsServerThemingMudBlazorModule)
    )]
public class AbpAspNetCoreComponentsServerMudBlazorBasicThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new MudBlazorBasicThemeToolbarContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorMudBlazorBasicThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(BlazorStandardBundles.Styles.Global)
                        .AddContributors(typeof(BlazorServerMudBlazorBasicThemeStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BlazorMudBlazorBasicThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(BlazorStandardBundles.Scripts.Global)
                        .AddContributors(typeof(BlazorServerMudBlazorBasicThemeScriptContributor));
                });
        });
    }
}
