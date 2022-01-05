using Markdig;
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

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Blogs;

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

    public BlogFeatureDto TagsFeature { get; private set; }

    protected IBlogPostPublicAppService BlogPostPublicAppService { get; }

    protected IBlogFeatureAppService BlogFeatureAppService { get; }

    public BlogPostModel(
        IBlogPostPublicAppService blogPostPublicAppService,
        IBlogFeatureAppService blogFeaturePublicAppService)
    {
        BlogPostPublicAppService = blogPostPublicAppService;
        BlogFeatureAppService = blogFeaturePublicAppService;
    }

    public virtual async Task OnGetAsync()
    {
        BlogPost = await BlogPostPublicAppService.GetAsync(BlogSlug, BlogPostSlug);

        if (GlobalFeatureManager.Instance.IsEnabled<CommentsFeature>())
        {
            CommentsFeature = await BlogFeatureAppService.GetOrDefaultAsync(BlogPost.BlogId, GlobalFeatures.CommentsFeature.Name);
        }

        if (GlobalFeatureManager.Instance.IsEnabled<ReactionsFeature>())
        {
            ReactionsFeature = await BlogFeatureAppService.GetOrDefaultAsync(BlogPost.BlogId, GlobalFeatures.ReactionsFeature.Name);
        }

        if (GlobalFeatureManager.Instance.IsEnabled<RatingsFeature>())
        {
            RatingsFeature = await BlogFeatureAppService.GetOrDefaultAsync(BlogPost.BlogId, GlobalFeatures.RatingsFeature.Name);
        }

        if (GlobalFeatureManager.Instance.IsEnabled<TagsFeature>())
        {
            TagsFeature = await BlogFeatureAppService.GetOrDefaultAsync(BlogPost.BlogId, GlobalFeatures.TagsFeature.Name);
        }
    }
}
