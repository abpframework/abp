using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class BloggingHttpApiModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BloggingResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
