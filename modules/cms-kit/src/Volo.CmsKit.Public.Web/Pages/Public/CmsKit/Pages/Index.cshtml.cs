using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Public.Pages;
using Volo.CmsKit.Web.Contents;
using Volo.CmsKit.Web.Pages;

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Pages;

public class IndexModel : CommonPageModel
{
    [BindProperty(SupportsGet = true)]
    public string Slug { get; set; }

    protected IPagePublicAppService PagePublicAppService { get; }

    protected ContentParser ContentParser { get; }

    public PageViewModel ViewModel { get; private set; }

    public IndexModel(IPagePublicAppService pagePublicAppService, ContentParser contentParser)
    {
        PagePublicAppService = pagePublicAppService;
        ContentParser = contentParser;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var pageDto = await PagePublicAppService.FindBySlugAsync(Slug);
        ViewModel = ObjectMapper.Map<PageDto, PageViewModel>(pageDto);
        if (ViewModel == null)
        {
            return NotFound();
        }
        
        ViewModel.ContentFragments = await ContentParser.ParseAsync(pageDto.Content);

        return Page();
    }
}
