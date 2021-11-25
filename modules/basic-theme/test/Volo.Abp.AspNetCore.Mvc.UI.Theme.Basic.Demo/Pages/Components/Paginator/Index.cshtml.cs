using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Demo.Pages.Components.Paginator;

public class IndexModel : PageModel
{
    public PagerModel PagerModel { get; set; }

    public void OnGet(int currentPage = 1, string sort = null)
    {
        PagerModel = new PagerModel(100, 10, currentPage, 10, "/Components/Paginator", sort);
    }
}
