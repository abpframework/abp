using Volo.Abp.Modularity;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Public;

[DependsOn(
    typeof(CmsKitCommonApplicationContractsModule),
    typeof(AbpEventBusModule)
    )]
public class CmsKitPublicApplicationContractsModule : AbpModule
{

}
