using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureManager : DomainService, IBlogFeatureManager
    {
        protected IBlogFeatureRepository BlogFeatureRepository { get; }

        public BlogFeatureManager(IBlogFeatureRepository blogFeatureRepository)
        {
            BlogFeatureRepository = blogFeatureRepository;
        }

        public async Task<List<BlogFeature>> GetListAsync(Guid blogId)
        {
            var blogFeatures = await BlogFeatureRepository.GetListAsync(blogId);

            blogFeatures.AddIfNotContains(new BlogFeature(blogId, BlogPostConsts.CommentsFeatureName));
            blogFeatures.AddIfNotContains(new BlogFeature(blogId, BlogPostConsts.ReactionsFeatureName));
            blogFeatures.AddIfNotContains(new BlogFeature(blogId, BlogPostConsts.RatingsFeatureName));

            return blogFeatures;
        }
    }
}
