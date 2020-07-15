using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitPublicApplicationContractsModule),
        typeof(CmsKitCommonHttpApiModule))]
    public class CmsKitPublicHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsKitPublicHttpApiModule).Assembly);
            });
        }

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
