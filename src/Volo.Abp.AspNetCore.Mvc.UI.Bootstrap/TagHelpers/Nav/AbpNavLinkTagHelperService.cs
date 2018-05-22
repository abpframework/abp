using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavLinkTagHelperService : AbpTagHelperService<AbpNavLinkTagHelper>
    {
        private readonly HtmlEncoder _encoder;

        public AbpNavLinkTagHelperService(HtmlEncoder encoder)
        {
            _encoder = encoder;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;
            SetClasses(context, output);

            if (!string.IsNullOrWhiteSpace(TagHelper.Href))
            {
                output.Attributes.Add("href", TagHelper.Href);
            }
        }

        protected virtual void SetClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("nav-link");

            SetDisabledClass(context, output);
            SetActiveClass(context, output);

            output.Attributes.RemoveAll("abp-nav-link");
        }

        protected virtual void SetDisabledClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Disabled ?? false)
            {
                output.Attributes.AddClass("disabled");
            }
        }

        protected virtual void SetActiveClass(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Active??false)
            {
                output.Attributes.AddClass("active");
            }
        }
    }
}