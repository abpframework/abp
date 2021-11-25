using Volo.Abp.Application.Services;
using Volo.Blogging.Localization;

namespace Volo.Blogging.Admin
{
    public abstract class BloggingAdminAppServiceBase : ApplicationService
    {
        protected BloggingAdminAppServiceBase()
        {
            ObjectMapperContext = typeof(BloggingAdminApplicationModule);
            LocalizationResource = typeof(BloggingResource);
        }
    }
}
