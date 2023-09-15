using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Public.Pages;

[RequiresFeature(CmsKitFeatures.PageEnable)]
[RequiresGlobalFeature(typeof(PagesFeature))]
public class PagePublicAppService : CmsKitPublicAppServiceBase, IPagePublicAppService
{
    protected IPageRepository PageRepository { get; }
    protected PageManager PageManager { get; }

    protected IDistributedCache<PageCacheItem, PageCacheKey> PageCache { get; }

    public PagePublicAppService(
        IPageRepository pageRepository,
        PageManager pageManager,
        IDistributedCache<PageCacheItem, PageCacheKey> pageCache)
    {
        PageRepository = pageRepository;
        PageManager = pageManager;
        PageCache = pageCache;
    }

    public virtual async Task<PageDto> FindBySlugAsync(string slug)
    {
        var pageCacheItem = await PageCache.GetOrAddAsync(new PageCacheKey(slug), async () =>
        {
            var page = await PageRepository.FindBySlugAsync(slug);
            if (page is null)
            {
                return null;
            }

            return ObjectMapper.Map<Page, PageCacheItem>(page);
        });

        if (pageCacheItem is null)
        {
            return null;
        }

        return ObjectMapper.Map<PageCacheItem, PageDto>(pageCacheItem);
    }

    public virtual async Task<PageDto> FindDefaultHomePageAsync()
    {
        var pageCacheItem = await PageCache.GetAsync(new PageCacheKey(PageConsts.DefaultHomePageCacheKey));
        if (pageCacheItem is null)
        {
            var page = await PageManager.GetHomePageAsync();
            if (page is null)
            {
                return null;
            }

            pageCacheItem = ObjectMapper.Map<Page, PageCacheItem>(page);

            await PageCache.SetAsync(new PageCacheKey(PageConsts.DefaultHomePageCacheKey), pageCacheItem,
                new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) });
        }

        return ObjectMapper.Map<PageCacheItem, PageDto>(pageCacheItem);
    }

    public async Task<bool> DoesSlugExistAsync([NotNull] string slug)
    {
        var cached = await PageCache.GetAsync(new PageCacheKey(slug));
        if (cached is not null)
        {
            return true;
        }

        return await PageRepository.ExistsAsync(slug);
    }
}
