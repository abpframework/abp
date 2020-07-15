using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.CmsKit.Admin;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(CmsKitPublicApplicationContractsModule),
        typeof(CmsKitAdminApplicationContractsModule)
        )]
    public class CmsKitApplicationContractsModule : AbpModule
    {

    }
}
