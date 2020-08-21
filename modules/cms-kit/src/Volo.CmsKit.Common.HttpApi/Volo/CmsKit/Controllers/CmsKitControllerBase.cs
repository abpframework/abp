using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Controllers
{
    public abstract class CmsKitControllerBase : AbpController
    {
        protected CmsKitControllerBase()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
