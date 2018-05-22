using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavBarTagHelperService : AbpTagHelperService<AbpNavBarTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.Attributes.AddClass("navbar");
            output.Attributes.AddClass("bg-light");
            output.Attributes.AddClass("navbar-light");

            SetSize(context,output);
        }

        protected virtual void SetSize(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Size != AbpNavbarSize.Default)
            {
                output.Attributes.AddClass("navbar-expand-" + TagHelper.Size.ToString().ToLowerInvariant());
            }
        }
    }
}