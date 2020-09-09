using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Public;

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
