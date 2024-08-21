using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
)]
public class AbpAspNetCoreComponentsMauiBlazorThemingBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.GlobalAssets.Enabled = true;
            options.GlobalAssets.GlobalStyleBundleName = MauiBlazorStandardBundles.Styles.Global;
            options.GlobalAssets.GlobalScriptBundleName = MauiBlazorStandardBundles.Scripts.Global;

            options
                .StyleBundles
                .Add(MauiBlazorStandardBundles.Styles.Global, bundle =>
                {
                    bundle.AddContributors(typeof(MauiStyleContributor));
                });

            options
                .ScriptBundles
                .Add(MauiBlazorStandardBundles.Scripts.Global, bundle =>
                {
                    bundle.AddContributors(typeof(MauiScriptContributor));
                });
        });
    }
}
