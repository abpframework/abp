using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

[HtmlTargetElement("abp-paginator")]
public class AbpPaginationTagHelper : AbpTagHelper<AbpPaginationTagHelper, AbpPaginationTagHelperService>
{
    public PagerModel Model { get; set; } = default!;

    public bool? ShowInfo { get; set; }

    public AbpPaginationTagHelper(AbpPaginationTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
