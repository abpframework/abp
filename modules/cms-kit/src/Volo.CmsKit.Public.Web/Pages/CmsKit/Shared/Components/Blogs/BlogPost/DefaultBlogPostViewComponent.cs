using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Blogs.BlogPost
{
    [ViewComponent(Name = "CmsDefaultBlogPost")]
    public class DefaultBlogPostViewComponent : AbpViewComponent
    {
        protected IBlogPostPublicAppService BlogPostPublicAppService { get; }

        public DefaultBlogPostViewComponent(IBlogPostPublicAppService blogPostPublicAppService)
        {
            BlogPostPublicAppService = blogPostPublicAppService;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(string blogUrlSlug, string blogPostUrlSlug)
        {
            var blogPost = await BlogPostPublicAppService.GetAsync(blogUrlSlug, blogPostUrlSlug);

            return View("~/Pages/CmsKit/Shared/Components/Blogs/BlogPost/Default.cshtml", blogPost);
        }
    }
}
