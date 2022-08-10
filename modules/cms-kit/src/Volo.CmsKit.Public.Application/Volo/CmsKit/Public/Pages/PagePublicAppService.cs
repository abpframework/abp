using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Contents;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Public.Pages;

[RequiresGlobalFeature(typeof(PagesFeature))]
public class PagePublicAppService : CmsKitPublicAppServiceBase, IPagePublicAppService
{
    protected IPageRepository PageRepository { get; }
    protected ContentParser ContentParser { get; }
    private readonly IDistributedCache<PageDto> _cache;

    public PagePublicAppService(IPageRepository pageRepository, ContentParser contentParser, IDistributedCache<PageDto> cache)
    {
        PageRepository = pageRepository;
        ContentParser = contentParser;
        _cache = cache;
    }

    public virtual async Task<PageDto> FindBySlugAsync(string slug)
    {
        var page = await PageRepository.FindBySlugAsync(slug);

        if (page == null)
        {
            return null;
        }

        var pageDto = ObjectMapper.Map<Page, PageDto>(page);
        pageDto.ContentFragments = await ContentParser.ParseAsync(page.Content);
        return pageDto;
    }

    public virtual async Task<PageDto> FindDefaultHomePageAsync()
    {
        var pageDto = await _cache.GetAsync("DefaultHomePage");
        if (pageDto is null)
        {
            var page = await PageRepository.FindByIsHomePageAsync(true);
            if (page is null)
            {
                return null;
            }

            pageDto = ObjectMapper.Map<Page, PageDto>(page);

            await _cache.SetAsync("DefaultHomePage", pageDto,
                new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) });
        }

        return pageDto;
    }
}
