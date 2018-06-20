using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card
{
    public class AbpCardLinkTagHelperService : AbpTagHelperService<AbpCardLinkTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("card-link");
            output.Attributes.RemoveAll("abp-card-link");
        }
    }
}