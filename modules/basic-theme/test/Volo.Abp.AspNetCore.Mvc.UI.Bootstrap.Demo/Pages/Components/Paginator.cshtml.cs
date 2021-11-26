using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Pages.Components;

public class PaginatorModel : PageModel
{
    public PagerModel PagerModel { get; set; }

    public void OnGet(int currentPage, string sort)
    {
        PagerModel = new PagerModel(100, 10, currentPage, 10, "/Components/Paginator", sort);
    }
}
