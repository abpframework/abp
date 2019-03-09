using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavbarToggleTagHelperService : AbpTagHelperService<AbpNavbarToggleTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetRandomNameIfNotProvided();
            output.TagName = "div";
            output.Attributes.AddClass("collapse");
            output.Attributes.AddClass("navbar-collapse");
            output.Attributes.Add("id",TagHelper.Id);
            SetToggleButton(context,output);
        }

        protected virtual void SetToggleButton(TagHelperContext context, TagHelperOutput output)
        {
            output.PreElement.SetHtmlContent(
                "<button class=\"navbar-toggler\" type=\"button\" data-toggle=\"collapse\" data-target=\"#" +
                TagHelper.Id + "\" aria-controls=\"" + TagHelper.Id +
                "\" aria-expanded=\"false\" aria-label=\"Toggle navigation\">\r\n    <span class=\"navbar-toggler-icon\"></span>\r\n  </button>");
        }

        protected virtual void SetRandomNameIfNotProvided()
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Id))
            {
                TagHelper.Id = "N" + Guid.NewGuid().ToString("N");
            }
        }
    }
}