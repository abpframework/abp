using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Table
{
    public class AbpTableStyleTagHelperService : AbpTagHelperService<AbpTableStyleTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetStyle(context,output);
        }

        protected virtual void SetStyle(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.AbpTableStyle != AbpTableStyle.Default)
            {
                output.Attributes.AddClass("table-" + TagHelper.AbpTableStyle.ToString().ToLowerInvariant());
            }
        }

        protected virtual void SetDarkTableStyle(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.AbpDarkTableStyle != AbpTableStyle.Default)
            {
                output.Attributes.AddClass("bg-" + TagHelper.AbpDarkTableStyle.ToString().ToLowerInvariant());
            }
        }
    }
}