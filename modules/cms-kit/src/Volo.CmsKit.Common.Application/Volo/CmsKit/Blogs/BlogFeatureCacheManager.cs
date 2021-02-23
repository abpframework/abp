using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureCacheManager : IBlogFeatureCacheManager, ITransientDependency
    {
        protected IDistributedCache<BlogFeatureDto> Cache { get; }

        public BlogFeatureCacheManager(IDistributedCache<BlogFeatureDto> cache)
        {
            Cache = cache;
        }

        public async Task<BlogFeatureDto> AddOrGetAsync(Guid blogId, string featureName, Func<Task<BlogFeatureDto>> factory)
        {
            return await Cache.GetOrAddAsync(
                GetCacheKey(blogId, featureName),
                factory,
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });
        }

        public Task ClearAsync(Guid blogId, string featureName)
        {
            return Cache.RemoveAsync(GetCacheKey(blogId, featureName));
        }

        private string GetCacheKey(Guid blogId, [NotNull] string featureName)
        {
            Check.NotNullOrWhiteSpace(featureName, nameof(featureName));

            return $"{blogId}_{featureName}";
        }
    }
}
