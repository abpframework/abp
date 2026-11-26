using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public;

public abstract class CmsKitBlogPostInteractionAppServiceBase : CmsKitPublicAppServiceBase
{
    protected IBlogPostRepository BlogPostRepository { get; }

    protected IBlogFeatureAppService BlogFeatureAppService { get; }

    protected CmsKitBlogPostInteractionAppServiceBase(
        IBlogPostRepository blogPostRepository,
        IBlogFeatureAppService blogFeatureAppService)
    {
        BlogPostRepository = blogPostRepository;
        BlogFeatureAppService = blogFeatureAppService;
    }

    protected virtual async Task CheckBlogFeatureAsync(string entityType, string entityId, string featureName)
    {
        // Definition stores treat culturally equivalent entity type names as the same definition.
        if (!string.Equals(entityType, BlogPostConsts.EntityType, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        if (!GlobalFeatureManager.Instance.IsEnabled<BlogsFeature>() || !Guid.TryParse(entityId, out var postId))
        {
            throw new EntityNotFoundException(typeof(BlogPost), entityId);
        }

        var post = await BlogPostRepository.GetAsync(postId);
        var feature = await BlogFeatureAppService.GetOrDefaultAsync(post.BlogId, featureName);
        if (!feature.IsEnabled)
        {
            throw new AbpAuthorizationException();
        }
    }
}
