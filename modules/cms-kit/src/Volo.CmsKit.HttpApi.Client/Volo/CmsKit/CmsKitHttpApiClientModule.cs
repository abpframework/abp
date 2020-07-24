using Volo.Abp.Modularity;

namespace Volo.CmsKit
{
    [DependsOn(
        typeof(CmsKitAdminHttpApiClientModule),
        typeof(CmsKitPublicHttpApiClientModule),
        typeof(CmsKitApplicationContractsModule)
        )]
    public class CmsKitHttpApiClientModule : AbpModule
    {

    }
}
