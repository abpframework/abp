using Volo.Abp.Application.Services;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Admin
{
    public abstract class AdminAppService : ApplicationService
    {
        protected AdminAppService()
        {
            LocalizationResource = typeof(CmsKitResource);
            ObjectMapperContext = typeof(AdminApplicationModule);
        }
    }
}
