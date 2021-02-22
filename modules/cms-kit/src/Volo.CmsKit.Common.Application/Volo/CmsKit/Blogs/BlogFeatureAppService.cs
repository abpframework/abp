using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Distributed;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureAppService : CmsKitAppServiceBase, IBlogFeatureAppService
    {
        protected IBlogFeatureRepository BlogFeatureRepository { get; }
        protected IBlogFeatureCacheManager BlogFeatureCacheManager { get; }

        public BlogFeatureAppService(
            IBlogFeatureRepository blogFeatureRepository,
            IBlogFeatureCacheManager blogFeatureCacheManager)
        {
            BlogFeatureRepository = blogFeatureRepository;
            BlogFeatureCacheManager = blogFeatureCacheManager;
        }

        public Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName)
        {
            return BlogFeatureCacheManager
                    .AddOrGetAsync(
                        blogId,
                        featureName,
                        ()=> GetOrDefaultFromDatabaseAsync(blogId, featureName)
                        );
        }

        private async Task<BlogFeatureDto> GetOrDefaultFromDatabaseAsync(Guid blogId, string featureName)
        {
            var feature = await BlogFeatureRepository.FindAsync(blogId, featureName);
            var blogFeature = feature ?? new BlogFeature(blogId, featureName);

            return ObjectMapper.Map<BlogFeature, BlogFeatureDto>(blogFeature);
        }
    }
}
