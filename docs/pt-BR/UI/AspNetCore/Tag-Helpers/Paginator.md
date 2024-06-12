# Paginador

## Introdução

`abp-paginator` é a tag abp para paginação. Requer um modelo do tipo `Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination.PagerModel`.

Uso básico:

````xml
<abp-paginator model="Model.PagerModel" show-info="true"></abp-paginator>
````

Modelo:

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



## Demonstração

Veja a [página de demonstração do paginador](https://bootstrap-taghelpers.abp.io/Components/Paginator) para vê-lo em ação.

## Atributos

### model

O modelo do tipo `Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination.PagerModel` pode ser inicializado com os seguintes dados:

* `totalCount`
* `shownItemsCount`
* `currentPage`
* `pageSize`
* `pageUrl`
* `sort` (padrão nulo)

### show-info

Um valor que indica se uma informação extra sobre o início, fim e total de registros será exibida. Deve ser um dos seguintes valores:

* `false` (valor padrão)
* `true`