using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class CmsKitCommonApplicationContractsModule : AbpModule
    {

    }
}
