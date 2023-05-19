using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Blogs;

public class BlogFeatureChangedHandler :
    ILocalEventHandler<EntityCreatedEventData<BlogFeature>>,
    ILocalEventHandler<EntityUpdatedEventData<BlogFeature>>,
    ITransientDependency
{
    protected IDistributedCache<BlogFeatureCacheItem, BlogFeatureCacheKey> Cache { get; }

    public BlogFeatureChangedHandler(
        IDistributedCache<BlogFeatureCacheItem, BlogFeatureCacheKey> cache)
    {
        Cache = cache;
    }

    public Task RemoveFromCacheAsync(Guid blogId, string featureName)
    {
        return Cache.RemoveAsync(new BlogFeatureCacheKey(blogId, featureName));
    }

    public Task HandleEventAsync(EntityCreatedEventData<BlogFeature> eventData)
    {
        return RemoveFromCacheAsync(eventData.Entity.BlogId, eventData.Entity.FeatureName);
    }

    public Task HandleEventAsync(EntityUpdatedEventData<BlogFeature> eventData)
    {
        return RemoveFromCacheAsync(eventData.Entity.BlogId, eventData.Entity.FeatureName);
    }
}
