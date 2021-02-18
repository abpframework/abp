using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Public.Blogs
{
    public class BlogFeaturePublicAppService : CmsKitPublicAppServiceBase, IBlogFeaturePublicAppService
    {
        protected IBlogFeatureRepository BlogFeatureRepository { get; }
        protected IDistributedCache<BlogFeatureDto> Cache { get; }

        public BlogFeaturePublicAppService(
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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
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
