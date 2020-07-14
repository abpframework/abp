using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitDomainSharedModule)
        )]
    public class CmsKitDomainModule : AbpModule
    {

    }
}
