using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Public;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitAdminHttpApiClientModule),
    typeof(CmsKitPublicHttpApiClientModule),
    typeof(CmsKitApplicationContractsModule)
    )]
public class CmsKitHttpApiClientModule : AbpModule
{

}
