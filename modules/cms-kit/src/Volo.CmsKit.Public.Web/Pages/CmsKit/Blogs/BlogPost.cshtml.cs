using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Blogs
{
    public class BlogPostModel : CmsKitPublicPageModelBase
    {
        [BindProperty(SupportsGet = true)]
        public string BlogSlug { get; set; }

        [BindProperty(SupportsGet = true)]
        public string BlogPostSlug { get; set; }

        public BlogPostPublicDto BlogPost { get; private set; }

        public BlogFeatureDto CommentsFeature { get; private set; }

        public BlogFeatureDto ReactionsFeature { get; private set; }

        public BlogFeatureDto RatingsFeature { get; private set; }

        protected IBlogPostPublicAppService BlogPostPublicAppService { get; }

        protected IBlogFeaturePublicAppService BlogFeaturePublicAppService { get; }

        public BlogPostModel(
            IBlogPostPublicAppService blogPostPublicAppService,
            IBlogFeaturePublicAppService blogFeaturePublicAppService)
        {
            BlogPostPublicAppService = blogPostPublicAppService;
            BlogFeaturePublicAppService = blogFeaturePublicAppService;
        }

        public virtual async Task OnGetAsync()
        {
            BlogPost = await BlogPostPublicAppService.GetAsync(BlogSlug, BlogPostSlug);

            if (GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>())
            {
                CommentsFeature = await BlogFeaturePublicAppService.GetOrDefaultAsync(BlogPost.BlogId, BlogPostConsts.CommentsFeatureName);
            }

            if (GlobalFeatureManager.Instance.IsEnabled<ReactionsFeature>())
            {
                ReactionsFeature = await BlogFeaturePublicAppService.GetOrDefaultAsync(BlogPost.BlogId, BlogPostConsts.ReactionsFeatureName);
            }

            if (GlobalFeatureManager.Instance.IsEnabled<RatingsFeature>())
            {
                RatingsFeature = await BlogFeaturePublicAppService.GetOrDefaultAsync(BlogPost.BlogId, BlogPostConsts.RatingsFeatureName);
            }
        }
    }
}
