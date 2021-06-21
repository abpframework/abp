using Volo.Abp.Application.Services;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit
{
    public abstract class CmsKitAppServiceBase : ApplicationService
    {
        protected CmsKitAppServiceBase()
        {
            LocalizationResource = typeof(CmsKitResource);
        }
    }
}
