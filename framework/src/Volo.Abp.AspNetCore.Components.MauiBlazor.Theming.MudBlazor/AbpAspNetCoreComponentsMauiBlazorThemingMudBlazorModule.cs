using Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;
using Volo.Abp.AspNetCore.Components.MauiBlazor;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsMauiBlazorThemingMudBlazorBundlingModule),
    typeof(AbpAspNetCoreComponentsWebThemingMudBlazorModule),
    typeof(AbpAspNetCoreComponentsMauiBlazorModule)
)]
public class AbpAspNetCoreComponentsMauiBlazorThemingMudBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(MauiBlazorMudBlazorStandardBundles.Styles.Global, bundle =>
                {
                    bundle.AddContributors(typeof(MauiBlazorMudBlazorStyleContributor));
                });

            options
                .ScriptBundles
                .Add(MauiBlazorMudBlazorStandardBundles.Scripts.Global, bundle =>
                {
                    bundle.AddContributors(typeof(MauiBlazorMudBlazorScriptContributor));
                });
        });
    }
}
