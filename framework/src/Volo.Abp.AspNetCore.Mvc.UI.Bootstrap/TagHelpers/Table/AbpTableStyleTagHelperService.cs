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
            if (TagHelper.TableStyle != AbpTableStyle.Default)
            {
                output.Attributes.AddClass("table-" + TagHelper.TableStyle.ToString().ToLowerInvariant());
            }
        }
    }
}