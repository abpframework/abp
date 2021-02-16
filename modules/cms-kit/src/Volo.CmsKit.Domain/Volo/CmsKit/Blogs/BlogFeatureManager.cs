using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureManager : DomainService, IBlogFeatureManager
    {
        protected IBlogFeatureRepository BlogFeatureRepository { get; }

        protected IDefaultBlogFeatureProvider DefaultBlogFeatureProvider { get; }

        public BlogFeatureManager(
            IBlogFeatureRepository blogFeatureRepository,
            IDefaultBlogFeatureProvider defaultBlogFeatureProvider)
        {
            BlogFeatureRepository = blogFeatureRepository;
            DefaultBlogFeatureProvider = defaultBlogFeatureProvider;
        }

        public async Task<List<BlogFeature>> GetListAsync(Guid blogId)
        {
            var blogFeatures = await BlogFeatureRepository.GetListAsync(blogId);

            var defaultFeatures = await DefaultBlogFeatureProvider.GetDefaultFeaturesAsync(blogId);

            defaultFeatures.ForEach(x => blogFeatures.AddIfNotContains(x));

            return blogFeatures;
        }
    }
}
