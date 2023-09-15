using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Pages;

public class PageChangedHandler :
    ILocalEventHandler<EntityCreatedEventData<Page>>,
    ILocalEventHandler<EntityUpdatedEventData<Page>>,
    ITransientDependency

{
    protected IDistributedCache<PageCacheItem, PageCacheKey> Cache { get; }

    public PageChangedHandler(IDistributedCache<PageCacheItem, PageCacheKey> cache)
    {
        Cache = cache;
    }

    public Task RemoveFromCacheAsync(string slug)
    {
        return Cache.RemoveAsync(new PageCacheKey(slug));
    }

    public Task HandleEventAsync(EntityCreatedEventData<Page> eventData)
    {
        return RemoveFromCacheAsync(eventData.Entity.Slug);
    }

    public Task HandleEventAsync(EntityUpdatedEventData<Page> eventData)
    {
        return RemoveFromCacheAsync(eventData.Entity.Slug);
    }
}
