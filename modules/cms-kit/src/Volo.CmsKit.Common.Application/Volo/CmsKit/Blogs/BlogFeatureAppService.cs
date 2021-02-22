using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureAppService : CmsKitAppServiceBase, IBlogFeatureAppService
    {
        protected IBlogFeatureRepository BlogFeatureRepository { get; }
        protected IDistributedCache<BlogFeatureDto> Cache { get; }

        public BlogFeatureAppService(
            IBlogFeatureRepository blogFeatureRepository,
            IDistributedCache<BlogFeatureDto> cache)
        {
            BlogFeatureRepository = blogFeatureRepository;
            Cache = cache;
        }

        public async Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName)
        {
            return await Cache.GetOrAddAsync(
                $"{blogId}_{featureName}",
                () => GetFromDatabaseAsync(blogId, featureName),
                () => new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                });
        }

        private async Task<BlogFeatureDto> GetFromDatabaseAsync(Guid blogId, string featureName)
        {
            var feature = await BlogFeatureRepository.FindAsync(blogId, featureName);
            if (feature == null)
            {
                feature = new BlogFeature(blogId, featureName);
            }

            return ObjectMapper.Map<BlogFeature, BlogFeatureDto>(feature);
        }
    }
}
