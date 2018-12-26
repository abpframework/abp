using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabLinkTagHelperService : AbpTagHelperService<AbpTabLinkTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetPlaceholderForNameIfNotProvided();
            
            var tabHeader = GetTabHeaderItem(context, output);

            var tabHeaderItems = GetValueFromContext<List<TabItem>>(context, TabItems);

            tabHeaderItems.Add(new TabItem(tabHeader, "", false, TagHelper.Name, TagHelper.ParentDropdownName, false));

            output.SuppressOutput();
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper.Name + "-tab";
            var href = TagHelper.Href;
            var title = TagHelper.Title;

            if (!string.IsNullOrWhiteSpace(TagHelper.ParentDropdownName))
            {
                return "<a class=\"dropdown-item\" id=\"" + id + "\" href=\"" + href + "\">" + title + "</a>";
            }

            return "<li class=\"nav-item\"><a class=\"nav-link" + AbpTabItemActivePlaceholder + "\" id=\"" + id + "\" href=\"" + href + "\">" +
                   title +
                   "</a></li>";
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