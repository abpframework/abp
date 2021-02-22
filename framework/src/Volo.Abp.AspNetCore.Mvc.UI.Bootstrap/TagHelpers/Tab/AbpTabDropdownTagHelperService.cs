using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabDropdownTagHelperService : AbpTagHelperService<AbpTabDropdownTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Name))
            {
                throw new Exception("Name of tab dropdown tag can not bu null or empty.");
            }

            await output.GetChildContentAsync();
            var tabHeader = GetTabHeaderItem(context, output);

            var tabHeaderItems = context.GetValue<List<TabItem>>(TabItems);

            tabHeaderItems.Add(new TabItem(tabHeader, "", false, TagHelper.Name, "", true));

            output.SuppressOutput();
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper.Name + "-tab";
            var link = TagHelper.Name;
            var title = TagHelper.Title;

            var anchor = new TagBuilder("a");
            anchor.AddCssClass("nav-link dropdown-toggle");
            anchor.Attributes.Add("id", id);
            anchor.Attributes.Add("data-toggle", "dropdown");
            anchor.Attributes.Add("href", "#" + link);
            anchor.Attributes.Add("role", "button");
            anchor.Attributes.Add("aria-haspopup", "true");
            anchor.Attributes.Add("aria-expanded", "false");
            anchor.InnerHtml.AppendHtml(title);

            var menu = new TagBuilder("div");
            menu.AddCssClass("dropdown-menu");
            menu.InnerHtml.Append(AbpTabDropdownItemsActivePlaceholder);

            var listItem = new TagBuilder("li");
            listItem.AddCssClass("nav-item dropdown");
            listItem.InnerHtml.AppendHtml(anchor);
            listItem.InnerHtml.AppendHtml(menu);

            return listItem.ToHtmlString();
        }
    }
}
