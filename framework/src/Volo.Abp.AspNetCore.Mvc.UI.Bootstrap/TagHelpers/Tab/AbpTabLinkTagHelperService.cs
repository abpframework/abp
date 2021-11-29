using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabLinkTagHelperService : AbpTagHelperService<AbpTabLinkTagHelper>
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetPlaceholderForNameIfNotProvided();

            var tabHeader = GetTabHeaderItem(context, output);

            var tabHeaderItems = context.GetValue<List<TabItem>>(TabItems);

            tabHeaderItems.Add(new TabItem(tabHeader, "", false, TagHelper.Name, TagHelper.ParentDropdownName, false));

            output.SuppressOutput();

            return Task.CompletedTask;
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper.Name + "-tab";
            var href = TagHelper.Href;
            var title = TagHelper.Title;

            if (!string.IsNullOrWhiteSpace(TagHelper.ParentDropdownName))
            {
                var anchor = new TagBuilder("a");
                anchor.AddCssClass("dropdown-item");
                anchor.Attributes.Add("id", id);
                anchor.Attributes.Add("href", href);
                anchor.InnerHtml.AppendHtml(title);

                return anchor.ToHtmlString();
            }
            else
            {
                var anchor = new TagBuilder("a");
                anchor.AddCssClass("nav-link " + AbpTabItemActivePlaceholder);
                anchor.Attributes.Add("id", id);
                anchor.Attributes.Add("href", href);
                anchor.InnerHtml.AppendHtml(title);

                var listItem = new TagBuilder("li");
                listItem.AddCssClass("nav-item");
                listItem.InnerHtml.AppendHtml(anchor);

                return listItem.ToHtmlString();
            }
        }

        protected virtual void SetPlaceholderForNameIfNotProvided()
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Name))
            {
                TagHelper.Name = TabItemNamePlaceHolder;
            }
        }
    }
}
