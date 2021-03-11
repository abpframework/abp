using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Blogs.BlogPost
{
    [Widget(StyleFiles = new[]
    {
        "/Pages/CmsKit/Shared/Components/Blogs/BlogPost/default.css"
    })]
    public class DefaultBlogPostViewComponent : AbpViewComponent
    {
        protected IBlogPostPublicAppService BlogPostPublicAppService { get; }

        public DefaultBlogPostViewComponent(IBlogPostPublicAppService blogPostPublicAppService)
        {
            BlogPostPublicAppService = blogPostPublicAppService;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync(string blogSlug, string blogPostSlug)
        {
            var blogPost = await BlogPostPublicAppService.GetAsync(blogSlug, blogPostSlug);

            return View("~/Pages/CmsKit/Shared/Components/Blogs/BlogPost/Default.cshtml", blogPost);
        }
    }
}
