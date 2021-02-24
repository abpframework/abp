using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureChangedHandler : ILocalEventHandler<BlogFeatureChangedEto>, ITransientDependency
    {
        protected IBlogFeatureCacheManager CacheManager { get; }

        public BlogFeatureChangedHandler(IBlogFeatureCacheManager cacheManager)
        {
            CacheManager = cacheManager;
        }

        public async Task HandleEventAsync(BlogFeatureChangedEto eventData)
        {
            await CacheManager.ClearAsync(eventData.BlogId, eventData.FeatureName);
        }
    }
}
