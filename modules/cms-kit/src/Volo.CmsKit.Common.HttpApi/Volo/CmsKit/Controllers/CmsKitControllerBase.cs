using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Controllers
{
    public class CmsKitControllerBase : AbpController
    {
        public CmsKitControllerBase()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
