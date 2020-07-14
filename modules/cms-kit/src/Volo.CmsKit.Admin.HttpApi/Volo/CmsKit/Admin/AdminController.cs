using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Admin
{
    public abstract class AdminController : AbpController
    {
        protected AdminController()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
