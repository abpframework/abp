using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Blogging.Admin.Pages
{
    public abstract class BloggingAdminPageModel : AbpPageModel
    {
        public BloggingAdminPageModel()
        {
            ObjectMapperContext = typeof(BloggingAdminWebModule);
        }
    }
}
