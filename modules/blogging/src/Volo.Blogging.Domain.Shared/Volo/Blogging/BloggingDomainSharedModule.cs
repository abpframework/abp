using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    [DependsOn(typeof(LocalizationModule))]
    public class BloggingDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<BloggingResource>("en");
            });
        }
    }
}
