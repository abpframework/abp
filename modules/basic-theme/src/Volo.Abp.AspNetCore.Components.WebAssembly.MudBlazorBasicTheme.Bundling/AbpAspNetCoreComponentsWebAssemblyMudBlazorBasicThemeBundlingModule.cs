using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorBundlingModule)
)]
public class AbpAspNetCoreComponentsWebAssemblyMudBlazorBasicThemeBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            var globalStyles = options.StyleBundles.Get(BlazorWebAssemblyMudBlazorStandardBundles.Styles.Global);
            globalStyles.AddContributors(typeof(WebAssemblyMudBlazorBasicThemeBundleStyleContributor));
        });
    }
}
