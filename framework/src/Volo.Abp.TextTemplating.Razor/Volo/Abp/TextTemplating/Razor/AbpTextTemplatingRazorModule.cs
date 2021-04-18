using Volo.Abp.Modularity;

namespace Volo.Abp.TextTemplating.Razor
{
    [DependsOn(
        typeof(AbpTextTemplatingAbstractionsModule)
    )]
    public class AbpTextTemplatingRazorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpTemplateRendererProviderOptions>(options =>
            {
                options.AddProvider<RazorTemplateRendererProvider>(RazorTemplateRendererProvider.ProviderName);
            });
        }
    }
}
