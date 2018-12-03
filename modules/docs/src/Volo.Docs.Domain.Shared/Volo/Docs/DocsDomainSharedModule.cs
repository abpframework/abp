using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Docs.Localization;

namespace Volo.Docs
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class DocsDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<DocsResource>("en");
            });
        }
    }
}
