using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Alert
{
    public class AbpAlertHeaderTagHelperService : AbpTagHelperService<AbpAlertHeaderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = GetHeaderSize(context, output);
            output.Attributes.AddClass("alert-heading");
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        protected virtual string GetHeaderSize(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Size == AbpAlertHeaderSize.Default)
            {
                return AbpAlertHeaderSize.h4.ToString();
            }

            return TagHelper.Size.ToString();
        }
    }
}