using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(CmsKitCommonApplicationContractsModule)
        )]
    public class CmsKitCommonHttpApiModule : AbpModule
    {
    }
}
