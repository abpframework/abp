# 分页器

## 介绍

`abp-paginator` 是分页器的abp标签. 需要 `Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination.PagerModel` 类型的模型.

基本用法:

````xml
<abp-paginator model="Model.PagerModel" show-info="true"></abp-paginator>
````

模型:

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

参阅[分页器demo页面](https://bootstrap-taghelpers.abp.io/Components/Paginator)查看示例.

## Attributes

### model

`Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination.PagerModel` 类型模型可以用以下数据初始化:

* `totalCount`
* `shownItemsCount`
* `currentPage`
* `pageSize`
* `pageUrl`
* `sort` (默认值为null)

### show-info

指定是否显示开始,结束和总记录的其他信息. 应为以下值之一:

* `false` (默认值)
* `true`
