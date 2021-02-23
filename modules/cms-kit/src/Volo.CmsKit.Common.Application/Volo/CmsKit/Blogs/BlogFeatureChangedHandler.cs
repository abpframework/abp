using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureChangedHandler : IDistributedEventHandler<BlogFeatureChangedEto>, ITransientDependency
    {
        protected IDistributedCache<BlogFeatureDto> Cache { get; }

        public BlogFeatureChangedHandler(IDistributedCache<BlogFeatureDto> cache)
        {
            Cache = cache;
        }

        public async Task HandleEventAsync(BlogFeatureChangedEto eventData)
        {
            await Cache.RemoveAsync($"{eventData.BlogId}_{eventData.FeatureName}");
        }
    }
}
