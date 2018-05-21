using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Breadcrumb;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavItemTagHelperService : AbpTagHelperService<AbpNavItemTagHelper>
    {
        private readonly HtmlEncoder _encoder;

        public AbpNavItemTagHelperService(HtmlEncoder encoder)
        {
            _encoder = encoder;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("href",TagHelper.Href);
            SetClasses(context, output);

            output.Content.SetHtmlContent(await output.GetChildContentAsync());

            var list = GetValueFromContext<List<NavItem>>(context, NavItemContents);

            list.Add(new NavItem
            {
                Html = SurroundTagWithNavItem(context, output,RenderTagHelperOutput(output, _encoder)),
                Active = TagHelper.Active??false
            });

            output.SuppressOutput();
        }

        protected virtual void SetClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("nav-link");

            SetDisabledClass(context, output);
            SetActiveClass(context, output);
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
            output.Attributes.AddClass(AbpNavItemActivePlaceholder);
        }

        protected virtual string SurroundTagWithNavItem(TagHelperContext context, TagHelperOutput output, string html)
        {
            return "<li class=\"nav-item "+ AbpNavItemResponsiveAlignPlaceholder +" " + AbpNavItemResponsiveFlexPlaceholder + "\">" + html + "</li>";
        }
    }
}