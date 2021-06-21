using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.CmsKit.Admin.MediaDescriptors;

namespace Volo.CmsKit.Admin
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
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.FormBodyBindingIgnoredTypes.Add(typeof(CreateMediaInputWithStream));
            });
        }
    }
}
