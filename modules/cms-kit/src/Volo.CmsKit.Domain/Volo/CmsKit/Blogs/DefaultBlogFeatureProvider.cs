using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Blogs
{
    public class DefaultBlogFeatureProvider : IDefaultBlogFeatureProvider, ITransientDependency
    {
        public virtual Task<List<BlogFeature>> GetDefaultFeaturesAsync(Guid blogId)
        {
            return Task.FromResult(new List<BlogFeature>
            {
                new BlogFeature(blogId, BlogPostConsts.CommentsFeatureName),
                new BlogFeature(blogId, BlogPostConsts.ReactionsFeatureName),
                new BlogFeature(blogId, BlogPostConsts.RatingsFeatureName),
                new BlogFeature(blogId, BlogPostConsts.TagsFeatureName),
            });
        }
    }
}
