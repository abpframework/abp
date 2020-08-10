using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Admin
{
    public abstract class CmsKitAdminController : AbpController
    {
        protected CmsKitAdminController()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
