using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.CmsKit.Public.Pages;
using Volo.CmsKit.Web.Pages;

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Pages;

public class IndexModel : CommonPageModel
{
    [BindProperty(SupportsGet = true)]
    public string Slug { get; set; }

    protected IPagePublicAppService PagePublicAppService { get; }

    public PageDto Page;

    public IndexModel(IPagePublicAppService pagePublicAppService)
    {
        PagePublicAppService = pagePublicAppService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Page = await PagePublicAppService.FindBySlugAsync(Slug);

        if (Page == null)
        {
            return NotFound();
        }

        return Page();
    }
}
