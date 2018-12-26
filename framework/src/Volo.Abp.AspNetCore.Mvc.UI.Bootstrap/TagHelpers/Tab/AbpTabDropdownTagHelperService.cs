using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

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

            var tabHeaderItems = GetValueFromContext<List<TabItem>>(context, TabItems);

            tabHeaderItems.Add(new TabItem(tabHeader, "", false, TagHelper.Name, "", true));

            output.SuppressOutput();
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper.Name + "-tab";
            var link = TagHelper.Name;
            var title = TagHelper.Title;

            return "<li class=\"nav-item dropdown\">" +
                   "<a class=\"nav-link dropdown-toggle\" id=\"" + id + "\" data-toggle=\"dropdown\" href=\"#" + link + "\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"false\">" +
                   title +
                   "</a>" +
                   "<div class=\"dropdown-menu\">" +
                   AbpTabDropdownItemsActivePlaceholder +
                   "</div>" +
                   "</li>";
        }
    }
}