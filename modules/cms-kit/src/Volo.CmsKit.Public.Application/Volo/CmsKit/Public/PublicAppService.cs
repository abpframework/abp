using Volo.Abp.Application.Services;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Public
{
    public abstract class PublicAppService : ApplicationService
    {
        protected PublicAppService()
        {
            LocalizationResource = typeof(CmsKitResource);
            ObjectMapperContext = typeof(PublicApplicationModule);
        }
    }
}
