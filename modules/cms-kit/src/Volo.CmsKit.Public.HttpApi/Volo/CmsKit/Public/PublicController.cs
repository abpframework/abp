using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Public
{
    public abstract class PublicController : AbpController
    {
        protected PublicController()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
