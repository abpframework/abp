using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitCommonApplicationContractsModule),
        typeof(CmsKitDomainModule),
        typeof(AbpDddApplicationModule)
        )]
    public class CmsKitCommonApplicationModule : AbpModule
    {
    }
}
