using Volo.CmsKit.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.CmsKit
{
    public abstract class CmsKitController : AbpController
    {
        protected CmsKitController()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
