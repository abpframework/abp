using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

public class AbpCardImageTagHelperService : AbpTagHelperService<AbpCardImageTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass(TagHelper.Position.ToClassName());
        output.Attributes.RemoveAll("abp-card-image");
    }
}
