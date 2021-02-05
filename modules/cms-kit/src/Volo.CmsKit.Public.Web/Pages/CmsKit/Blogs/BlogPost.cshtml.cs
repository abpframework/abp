using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Blogs
{
    public class BlogPostModel : CmsKitPublicPageModelBase
    {
        [BindProperty(SupportsGet = true)]
        public string BlogUrlSlug { get; set; }

        [BindProperty(SupportsGet = true)]
        public string BlogPostUrlSlug { get; set; }

        public BlogPostPublicDto BlogPost { get; private set; }

        protected IBlogPostPublicAppService BlogPostPublicAppService { get; }

        public BlogPostModel(IBlogPostPublicAppService blogPostPublicAppService)
        {
            BlogPostPublicAppService = blogPostPublicAppService;
        }

        public async Task OnGetAsync()
        {
            BlogPost = await BlogPostPublicAppService.GetAsync(BlogUrlSlug, BlogPostUrlSlug);
        }
    }
}
