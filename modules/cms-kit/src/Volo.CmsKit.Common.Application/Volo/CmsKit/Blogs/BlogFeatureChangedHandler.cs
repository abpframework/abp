using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureChangedHandler : ILocalEventHandler<BlogFeatureChangedEto>, ITransientDependency
    {
        protected IDistributedCache<BlogFeatureCacheItem, BlogFeatureCacheKey> Cache { get; }

        public BlogFeatureChangedHandler(
            IDistributedCache<BlogFeatureCacheItem, BlogFeatureCacheKey> cache)
        {
            Cache = cache;
        }

        public async Task HandleEventAsync(BlogFeatureChangedEto eventData)
        {
            await Cache.RemoveAsync(new BlogFeatureCacheKey(eventData.BlogId, eventData.FeatureName));
        }
    }
}
