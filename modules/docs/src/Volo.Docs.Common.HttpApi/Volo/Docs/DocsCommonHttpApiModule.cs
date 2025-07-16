using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Docs.Common;
using Volo.Docs.Localization;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsCommonApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class DocsCommonHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(DocsCommonHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<DocsResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}