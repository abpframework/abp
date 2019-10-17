using Volo.Abp.Application.Services;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    public abstract class BloggingAppServiceBase : ApplicationService
    {
        protected BloggingAppServiceBase()
        {
            ObjectMapperContext = typeof(BloggingApplicationModule);
            LocalizationResource = typeof(BloggingResource);
        }
    }
}