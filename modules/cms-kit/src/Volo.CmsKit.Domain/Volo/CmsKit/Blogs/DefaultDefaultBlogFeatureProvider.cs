using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Blogs
{
    public class DefaultDefaultBlogFeatureProvider : IDefaultBlogFeatureProvider, ITransientDependency
    {
        public Task<List<BlogFeature>> GetDefaultFeaturesAsync(Guid blogId)
        {
            return Task.FromResult(new List<BlogFeature>
            {
                new BlogFeature(blogId, BlogPostConsts.CommentsFeatureName),
                new BlogFeature(blogId, BlogPostConsts.ReactionsFeatureName),
                new BlogFeature(blogId, BlogPostConsts.RatingsFeatureName),
                new BlogFeature(blogId, BlogPostConsts.RatingsFeatureName),
            });
        }
    }
}
