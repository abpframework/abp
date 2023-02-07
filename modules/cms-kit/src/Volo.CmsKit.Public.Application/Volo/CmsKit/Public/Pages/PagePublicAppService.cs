using System;
using System.Threading.Tasks;
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

    protected IDistributedCache<PageCacheItem> PageCache { get; }

    public PagePublicAppService(IPageRepository pageRepository, PageManager pageManager, IDistributedCache<PageCacheItem> pageCache)
    {
        PageRepository = pageRepository;
        PageManager = pageManager;
		PageCache = pageCache;
    }

    public virtual async Task<PageDto> FindBySlugAsync(string slug)
    {
        var page = await PageRepository.FindBySlugAsync(slug);
        if (page == null)
        {
            return null;
        }

        return ObjectMapper.Map<Page, PageDto>(page);
    }

    public virtual async Task<PageDto> FindDefaultHomePageAsync()
    {
        var pageCacheItem = await PageCache.GetAsync(PageConsts.DefaultHomePageCacheKey);
        if (pageCacheItem is null)
        {
            var page = await PageManager.GetHomePageAsync();
            if (page is null)
            {
                return null;
            }

            pageCacheItem = ObjectMapper.Map<Page, PageCacheItem>(page);

            await PageCache.SetAsync(PageConsts.DefaultHomePageCacheKey, pageCacheItem,
                new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) });
        }

        return ObjectMapper.Map<PageCacheItem, PageDto>(pageCacheItem);
    }
}
