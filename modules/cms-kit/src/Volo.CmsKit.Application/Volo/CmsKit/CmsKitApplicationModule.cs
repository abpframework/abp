using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitPublicApplicationModule),
        typeof(CmsKitAdminApplicationModule),
        typeof(CmsKitApplicationContractsModule)
        )]
    public class CmsKitApplicationModule : AbpModule
    {

    }
}
