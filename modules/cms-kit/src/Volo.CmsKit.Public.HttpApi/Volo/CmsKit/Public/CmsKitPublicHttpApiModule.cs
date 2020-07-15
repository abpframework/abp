using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Public
{
    [DependsOn(
        typeof(CmsKitPublicApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
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
