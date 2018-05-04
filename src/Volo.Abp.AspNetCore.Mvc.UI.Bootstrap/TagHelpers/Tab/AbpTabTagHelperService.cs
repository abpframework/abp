using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

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

            var tabHeaderItems = GetValueFromContext<List<TabItem>>(context, TabItems);

            var active = TagHelper.Active ?? false;

            tabHeaderItems.Add(new TabItem(tabHeader, tabContent,active));

            output.SuppressOutput();
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper.Name + "-tab";
            var link = TagHelper.Name;
            var control = TagHelper.Name;
            var title = TagHelper.Title;

            return "<a class=\"nav-item nav-link" + AbpTabItemActivePlaceholder + "\" id=\"" + id + "\" data-toggle=\"" + TabItemsDataTogglePlaceHolder + "\" href=\"#" + link + "\" role=\"tab\" aria-controls=\"" + control + "\" aria-selected=\"" + AbpTabItemSelectedPlaceholder + "\">" +
                   title +
                   "</a>";
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