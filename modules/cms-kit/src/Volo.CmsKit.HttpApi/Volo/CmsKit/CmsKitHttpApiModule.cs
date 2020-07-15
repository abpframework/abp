using Volo.Abp.Modularity;

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
