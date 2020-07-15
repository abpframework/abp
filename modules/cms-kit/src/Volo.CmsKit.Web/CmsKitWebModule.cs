using Volo.Abp.Modularity;
using Volo.CmsKit.Admin.Web;

namespace Volo.CmsKit.Web
{
    [DependsOn(
        typeof(CmsKitPublicWebModule),
        typeof(CmsKitAdminWebModule),
        typeof(CmsKitHttpApiModule)
        )]
    public class CmsKitWebModule : AbpModule
    {
    }
}
