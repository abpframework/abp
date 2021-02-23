using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Distributed;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureAppService : CmsKitAppServiceBase, IBlogFeatureAppService
    {
        protected virtual IBlogFeatureRepository BlogFeatureRepository { get; }

        protected virtual IBlogFeatureCacheManager BlogFeatureCacheManager { get; }

        public BlogFeatureAppService(
            IBlogFeatureRepository blogFeatureRepository,
            IBlogFeatureCacheManager blogFeatureCacheManager)
        {
            BlogFeatureRepository = blogFeatureRepository;
            BlogFeatureCacheManager = blogFeatureCacheManager;
        }

        public virtual Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName)
        {
            return BlogFeatureCacheManager
                    .AddOrGetAsync(
                        blogId,
                        featureName,
                        ()=> GetOrDefaultFroRepositoryAsync(blogId, featureName)
                        );
        }

        protected virtual async Task<BlogFeatureDto> GetOrDefaultFroRepositoryAsync(Guid blogId, string featureName)
        {
            var feature = await BlogFeatureRepository.FindAsync(blogId, featureName);
            var blogFeature = feature ?? new BlogFeature(blogId, featureName);

            return ObjectMapper.Map<BlogFeature, BlogFeatureDto>(blogFeature);
        }
    }
}
