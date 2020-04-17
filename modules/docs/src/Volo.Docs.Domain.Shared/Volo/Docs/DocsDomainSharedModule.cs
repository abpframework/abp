using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
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
            
            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Volo.Docs.Domain", typeof(DocsResource));
            });
        }
    }
}
