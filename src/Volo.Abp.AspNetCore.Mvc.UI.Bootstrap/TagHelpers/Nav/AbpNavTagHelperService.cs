using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Breadcrumb;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavTagHelperService : AbpTagHelperService<AbpNavTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("nav");
            SetAlign(context, output);
            SetNavStyle(context, output);
            SetResponsiveness(context, output);

            var list = InitilizeFormGroupContentsContext(context, output);

            await output.GetChildContentAsync();


            SetInnerList(context, output, list);
        }

        protected virtual void SetInnerList(TagHelperContext context, TagHelperOutput output, List<NavItem> list)
        {
            SetFirstOneActiveIfThereIsNotAny(context, output, list);

            var html = new StringBuilder("");

            foreach (var navItem in list)
            {
                var htmlPart = SetPlaceHolderClassesIfActiveAndGetHtml(navItem);

                html.AppendLine(htmlPart);
            }

            output.Content.SetHtmlContent(html.ToString());
        }

        protected virtual string SetPlaceHolderClassesIfActiveAndGetHtml(NavItem item)
        {
            var html = item.Html;

            html = item.Active ?
                item.Html.Replace(AbpNavItemActivePlaceholder, " active") :
                item.Html.Replace(AbpNavItemActivePlaceholder, "");

            html = TagHelper.Responsive?? false? html.Replace(AbpNavItemResponsiveFlexPlaceholder, "flex-sm-fill").Replace(AbpNavItemResponsiveAlignPlaceholder, "text-sm-center") :
                html.Replace(AbpNavItemResponsiveFlexPlaceholder, "").Replace(AbpNavItemResponsiveAlignPlaceholder, ""); 

            return html;
        }

        protected virtual void SetFirstOneActiveIfThereIsNotAny(TagHelperContext context, TagHelperOutput output, List<NavItem> list)
        {
            if (list.Count > 0 && !list.Any(bc => bc.Active))
            {
                list.FirstOrDefault().Active = true;
            }
        }

        protected virtual void SetResponsiveness(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Responsive??false)
            {
                output.Attributes.AddClass("flex-sm-row");
                output.Attributes.AddClass("flex-column");
            }
        }

        protected virtual List<NavItem> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<NavItem>();
            context.Items[NavItemContents] = items;
            return items;
        }

        protected virtual void SetAlign(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Align == AbpNavAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("justify-content-" + TagHelper.Align.ToString().ToLowerInvariant());
        }

        protected virtual void SetNavStyle(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.NavStyle)
            {
                case NavStyle.Default:
                    return;
                case NavStyle.Pill:
                    output.Attributes.AddClass("nav-pills");
                    break;
                case NavStyle.Vertical:
                    output.Attributes.AddClass("flex-column");
                    break;
                case NavStyle.PillVertical:
                    output.Attributes.AddClass("nav-pills");
                    output.Attributes.AddClass("flex-column");
                    break;
            }
        }
    }
}