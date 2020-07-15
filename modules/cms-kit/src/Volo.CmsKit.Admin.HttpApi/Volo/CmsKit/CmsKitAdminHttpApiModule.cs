using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitAdminApplicationContractsModule),
        typeof(CmsKitCommonHttpApiModule)
        )]
    public class CmsKitAdminHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsKitAdminHttpApiModule).Assembly);
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
