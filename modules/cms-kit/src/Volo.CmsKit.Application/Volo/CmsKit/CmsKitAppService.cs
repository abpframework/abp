using Volo.CmsKit.Localization;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit
{
    public abstract class CmsKitAppService : ApplicationService
    {
        protected CmsKitAppService()
        {
            LocalizationResource = typeof(CmsKitResource);
            ObjectMapperContext = typeof(CmsKitApplicationModule);
        }
    }
}
