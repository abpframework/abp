using System.Threading.Tasks;
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
    protected ContentParser ContentParser { get; }

    public PagePublicAppService(IPageRepository pageRepository, ContentParser contentParser)
    {
        PageRepository = pageRepository;
        ContentParser = contentParser;
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
}
