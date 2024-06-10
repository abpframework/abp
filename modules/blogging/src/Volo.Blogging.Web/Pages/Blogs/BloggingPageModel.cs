using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Pages.Blogs
{
    public abstract class BloggingPageModel : AbpPageModel
    {
        public BloggingPageModel()
        {
            ObjectMapperContext = typeof(BloggingWebModule);
        }
        
        protected async Task<BlogDto> GetBlogAsync(IBlogAppService blogAppService, BloggingUrlOptions blogOptions, string blogShortName)
        {
            if (!blogOptions.SingleBlogMode.Enabled)
            {
                return await blogAppService.GetByShortNameAsync(blogShortName);
            }

            if (!blogOptions.SingleBlogMode.BlogName.IsNullOrEmpty())
            {
                return await blogAppService.GetByShortNameAsync(blogOptions.SingleBlogMode.BlogName);
            }

            var blogs = await blogAppService.GetListAsync();
            return blogs.Items.Count == 1 ? blogs.Items[0] : blogs.Items.SingleOrDefault(x => x.ShortName == blogShortName);
        }
    }
}