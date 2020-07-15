using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(CmsKitCommonApplicationContractsModule)
        )]
    public class CmsKitCommonHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<CmsKitResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
