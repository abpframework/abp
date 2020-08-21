using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Public;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitAdminApplicationModule),
        typeof(CmsKitPublicApplicationModule),
        typeof(CmsKitApplicationContractsModule)
        )]
    public class CmsKitHttpApiModule : AbpModule
    {

    }
}
