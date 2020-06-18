using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Localization;

namespace Volo.Blogging.Areas.Blog.Controllers
{
    public abstract class BloggingControllerBase : AbpController
    {
        protected BloggingControllerBase()
        {
            ObjectMapperContext = typeof(BloggingWebModule);
            LocalizationResource = typeof(BloggingResource);
        }
    }
}