using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabTagHelperService : AbpTagHelperService<AbpTabTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetPlaceholderForNameIfNotProvided();

            var innerContent = await output.GetChildContentAsync();
            var tabHeader = GetTabHeaderItem(context, output);
            var tabContent = GetTabContentItem(innerContent.GetContent());

            var tabHeaderItems = context.GetValue<List<TabItem>>(TabItems);

            var active = TagHelper.Active ?? false;

            tabHeaderItems.Add(new TabItem(tabHeader, tabContent, active, TagHelper.Name, TagHelper.ParentDropdownName, false));

            output.SuppressOutput();
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper.Name + "-tab";
            var link = TagHelper.Name;
            var control = TagHelper.Name;
            var title = TagHelper.Title;

            if (!string.IsNullOrWhiteSpace(TagHelper.ParentDropdownName))
            {
                return "<a class=\"dropdown-item\" id=\"" + id + "\" href=\"#" + link + "\" data-toggle=\"tab\"  role=\"tab\" aria-controls=\"" + control + "\" aria-selected=\"false\">" + title + "</a>";
            }

            return "<li class=\"nav-item\"><a class=\"nav-link" + AbpTabItemActivePlaceholder + "\" id=\"" + id + "\" data-toggle=\"" + TabItemsDataTogglePlaceHolder + "\" href=\"#" + link + "\" role=\"tab\" aria-controls=\"" + control + "\" aria-selected=\"" + AbpTabItemSelectedPlaceholder + "\">" +
                   title +
                   "</a></li>";
        }

        protected virtual string GetTabContentItem(string content)
        {
            var headerId = TagHelper.Name + "-tab";
            var id = TagHelper.Name;

            return "<div class=\"tab-pane fade" + AbpTabItemShowActivePlaceholder + "\" id=\"" + id + "\" role=\"tabpanel\" aria-labelledby=\"" + headerId + "\">" +
                   content +
                   "</div>";
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