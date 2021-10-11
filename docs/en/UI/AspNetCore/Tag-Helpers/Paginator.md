# Paginator

## Introduction

`abp-paginator` is the abp tag for pagination. Requires `Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination.PagerModel` type of model.

Basic usage:

````xml
<abp-paginator model="Model.PagerModel" show-info="true"></abp-paginator>
````

Model:

````csharp
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Pages.Components
{
    public class PaginatorModel : PageModel
    {
        public PagerModel PagerModel { get; set; }

        public void OnGet(int currentPage, string sort)
        {
            PagerModel = new PagerModel(100, 10, currentPage, 10, "/Components/Paginator", sort);
        }
    }
}
````



## Demo

See the [paginator demo page](https://bootstrap-taghelpers.abp.io/Components/Paginator) to see it in action.

## Attributes

### model

`Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination.PagerModel` type of model can be initialized with the following data:

* `totalCount`
* `shownItemsCount`
* `currentPage`
* `pageSize`
* `pageUrl`
* `sort` (default null)

### show-info

A value indicates if an extra information about start, end and total records will be displayed. Should be one of the following values:

* `false` (default value)
* `true`
