using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Volo.CmsKit.Admin
{
    [DependsOn(
        typeof(CmsKitDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class AdminApplicationContractsModule : AbpModule
    {

    }
}
