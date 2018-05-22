using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavbarTextTagHelperService : AbpTagHelperService<AbpNavbarTextTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("navbar-text");
            output.Attributes.RemoveAll("abp-navbar-text");
        }
    }
}