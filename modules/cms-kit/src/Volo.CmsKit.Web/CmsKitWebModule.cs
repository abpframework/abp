using Volo.Abp.Modularity;
using Volo.CmsKit.Admin.Web;
using Volo.CmsKit.Public.Web;

namespace Volo.CmsKit.Web
{
    [DependsOn(
        typeof(CmsKitPublicWebModule),
        typeof(CmsKitAdminWebModule),
        typeof(CmsKitApplicationContractsModule)
        )]
    public class CmsKitWebModule : AbpModule
    {
    }
}
