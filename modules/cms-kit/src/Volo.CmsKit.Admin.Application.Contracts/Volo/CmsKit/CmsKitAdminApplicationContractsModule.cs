using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitCommonApplicationContractsModule)
        )]
    public class CmsKitAdminApplicationContractsModule : AbpModule
    {

    }
}
