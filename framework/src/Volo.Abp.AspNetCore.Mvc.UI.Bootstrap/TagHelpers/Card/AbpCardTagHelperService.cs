using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card
{
    public class AbpCardTagHelperService : AbpTagHelperService<AbpCardTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("card");

            SetBorder(context, output);
        }
        protected virtual void SetBorder(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Border == AbpCardBorderColorType.Default)
            {
                return;
            }

            output.Attributes.AddClass("border-" + TagHelper.Border.ToString().ToLowerInvariant());
        }
    }
}