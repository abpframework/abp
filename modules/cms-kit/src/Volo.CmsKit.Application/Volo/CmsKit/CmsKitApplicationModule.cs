using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Public;

namespace Volo.CmsKit;

[DependsOn(
    typeof(CmsKitPublicApplicationModule),
    typeof(CmsKitAdminApplicationModule),
    typeof(CmsKitApplicationContractsModule)
    )]
public class CmsKitApplicationModule : AbpModule
{

}
