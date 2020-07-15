using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitCommonApplicationContractsModule),
        typeof(CmsKitDomainModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class CmsKitCommonApplicationModule : AbpModule
    {
    }
}
