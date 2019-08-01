using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Docs.Localization;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsAdminApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class DocsAdminHttpApiModule : AbpModule
    {

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
