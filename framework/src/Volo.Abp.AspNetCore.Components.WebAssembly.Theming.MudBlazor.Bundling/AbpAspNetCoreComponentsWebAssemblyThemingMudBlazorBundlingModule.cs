using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
)]
public class AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.GlobalAssets.Enabled = true;
            options.GlobalAssets.GlobalStyleBundleName = BlazorWebAssemblyMudBlazorStandardBundles.Styles.Global;
            options.GlobalAssets.GlobalScriptBundleName = BlazorWebAssemblyMudBlazorStandardBundles.Scripts.Global;

            options
                .StyleBundles
                .Add(BlazorWebAssemblyMudBlazorStandardBundles.Styles.Global, bundle =>
                {
                    bundle.AddContributors(typeof(BlazorWebAssemblyMudBlazorStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BlazorWebAssemblyMudBlazorStandardBundles.Scripts.Global, bundle =>
                {
                    bundle.AddContributors(typeof(BlazorWebAssemblyMudBlazorScriptContributor));
                });

            options.MinificationIgnoredFiles.Add("_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js");
        });
    }
}
