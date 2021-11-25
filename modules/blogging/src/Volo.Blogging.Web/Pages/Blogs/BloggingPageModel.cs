using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Blogging.Pages.Blog
{
    public abstract class BloggingPageModel : AbpPageModel
    {
        public BloggingPageModel()
        {
            ObjectMapperContext = typeof(BloggingWebModule);
        }
    }
}