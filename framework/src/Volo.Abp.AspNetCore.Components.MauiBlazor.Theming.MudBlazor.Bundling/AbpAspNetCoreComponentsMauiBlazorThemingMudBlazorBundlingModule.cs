using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
)]
public class AbpAspNetCoreComponentsMauiBlazorThemingMudBlazorBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.GlobalAssets.Enabled = true;
            options.GlobalAssets.GlobalStyleBundleName = MauiBlazorMudBlazorStandardBundles.Styles.Global;
            options.GlobalAssets.GlobalScriptBundleName = MauiBlazorMudBlazorStandardBundles.Scripts.Global;

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
