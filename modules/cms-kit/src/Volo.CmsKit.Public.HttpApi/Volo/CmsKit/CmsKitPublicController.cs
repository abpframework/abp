using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit
{
    public abstract class CmsKitPublicController : AbpController
    {
        protected CmsKitPublicController()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
