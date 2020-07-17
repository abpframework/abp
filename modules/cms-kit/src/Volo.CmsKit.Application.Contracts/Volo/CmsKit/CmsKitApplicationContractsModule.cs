using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitPublicApplicationContractsModule),
        typeof(CmsKitAdminApplicationContractsModule)
        )]
    public class CmsKitApplicationContractsModule : AbpModule
    {

    }
}
