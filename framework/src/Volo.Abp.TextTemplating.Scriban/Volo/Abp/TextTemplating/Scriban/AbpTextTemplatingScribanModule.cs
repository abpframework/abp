using Volo.Abp.Modularity;

namespace Volo.Abp.TextTemplating.Scriban
{
    [DependsOn(
        typeof(AbpTextTemplatingAbstractionsModule)
    )]
    public class AbpTextTemplatingScribanModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpTemplateRendererProviderOptions>(options =>
            {
                options.AddProvider<ScribanTemplateRendererProvider>(ScribanTemplateRendererProvider.ProviderName);
            });
        }
    }
}
