using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Pages.Components
{
    public class PaginatorModel : PageModel
    {
        public PagerModel PagerModel { get; set; }

        public void OnGet(int currentPage, string sort)
        {
            PagerModel = new PagerModel(totalCount: 100,
                shownItemsCount: 10,
                currentPage: currentPage,
                pageSize: 10,
                pageUrl: "Paginator",
                sort: sort
            );
        }
    }
}