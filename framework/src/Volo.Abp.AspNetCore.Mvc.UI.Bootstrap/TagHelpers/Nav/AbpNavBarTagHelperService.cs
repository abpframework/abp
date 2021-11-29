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

            SetSize(context,output);
            SetStyle(context, output);
        }

        protected virtual void SetSize(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Size != AbpNavbarSize.Default)
            {
                output.Attributes.AddClass("navbar-expand-" + TagHelper.Size.ToString().ToLowerInvariant());
            }
        }

        protected virtual void SetStyle(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.NavbarStyle == AbpNavbarStyle.Default)
            {
                return;
            }

            var styleAsStringArray = TagHelper.NavbarStyle.ToString().ToLowerInvariant().Split('_');

            if (styleAsStringArray.Length < 2)
            {
                output.Attributes.AddClass("navbar-" + styleAsStringArray[0]);
            }
            else
            {
                output.Attributes.AddClass("navbar-" + styleAsStringArray[0]);
                output.Attributes.AddClass("bg-" + styleAsStringArray[1]);
            }
        }
    }
}