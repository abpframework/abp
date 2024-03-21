using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Pages.Blog
{
    public abstract class BloggingPageModel : AbpPageModel
    {
        public BloggingPageModel()
        {
            ObjectMapperContext = typeof(BloggingWebModule);
        }
        
        protected async Task<BlogDto> GetBlogAsync(IBlogAppService blogAppService, BloggingUrlOptions blogOptions, string blogShortName)
        {
            if (!blogOptions.SingleBlogMode.Enable)
            {
                return await blogAppService.GetByShortNameAsync(blogShortName);
            }

            if (!blogOptions.SingleBlogMode.BlogName.IsNullOrEmpty())
            {
                return await blogAppService.GetByShortNameAsync(blogOptions.SingleBlogMode.BlogName);
            }

            var blogs = await blogAppService.GetListAsync();
            if (blogs.Items.Count == 1)
            {
                return blogs.Items[0];
            }
            
            return await blogAppService.GetByShortNameAsync(blogOptions.SingleBlogMode.BlogName);
        }
    }
}