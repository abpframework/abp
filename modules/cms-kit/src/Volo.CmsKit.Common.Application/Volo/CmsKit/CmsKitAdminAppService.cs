using Volo.Abp.Application.Services;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit
{
    public abstract class CmsKitAppService : ApplicationService
    {
        protected CmsKitAppService()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
