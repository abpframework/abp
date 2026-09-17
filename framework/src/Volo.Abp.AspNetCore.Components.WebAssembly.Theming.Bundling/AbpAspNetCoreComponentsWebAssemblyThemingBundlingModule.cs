using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
)]
public class AbpAspNetCoreComponentsWebAssemblyThemingBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.GlobalAssets.Enabled = true;
            options.GlobalAssets.GlobalStyleBundleName = BlazorWebAssemblyStandardBundles.Styles.Global;
            options.GlobalAssets.GlobalScriptBundleName = BlazorWebAssemblyStandardBundles.Scripts.Global;

            options
                .StyleBundles
                .Add(BlazorWebAssemblyStandardBundles.Styles.Global, bundle =>
                {
                    bundle.AddContributors(typeof(BlazorWebAssemblyStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BlazorWebAssemblyStandardBundles.Scripts.Global, bundle =>
                {
                    bundle.AddContributors(typeof(BlazorWebAssemblyScriptContributor));
                });

            options.MinificationIgnoredFiles.Add("_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js");
        });
    }
}
