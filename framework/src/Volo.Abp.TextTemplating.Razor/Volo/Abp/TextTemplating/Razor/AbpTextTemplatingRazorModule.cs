using System;
using Volo.Abp.Modularity;

namespace Volo.Abp.TextTemplating.Razor;

[DependsOn(
    typeof(AbpTextTemplatingCoreModule)
)]
public class AbpTextTemplatingRazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpTextTemplatingOptions>(options =>
        {
            if (options.DefaultRenderingEngine.IsNullOrWhiteSpace())
            {
                options.DefaultRenderingEngine = RazorTemplateRenderingEngine.EngineName;
            }
            options.RenderingEngines[RazorTemplateRenderingEngine.EngineName] = typeof(RazorTemplateRenderingEngine);
        });
    }
}
