using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Public;

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
}
