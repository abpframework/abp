using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitCommonApplicationContractsModule),
    typeof(CmsKitDomainModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule)
)]
public class CmsKitCommonApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<CmsKitCommonApplicationModule>();
    }
}
