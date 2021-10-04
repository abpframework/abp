using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Admin
{
    public abstract class CmsKitAdminController : AbpControllerBase
    {
        protected CmsKitAdminController()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
