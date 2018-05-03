using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabItemTagHelperService : AbpTagHelperService<AbpTabItemTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var innerContent = await output.GetChildContentAsync();
            var tabHeader = GetTabHeaderItem();
            var tabContent = GetTabContentItem(innerContent.GetContent());
            
            var tabHeaderItems = GetValueFromContext<List<TabItem>>(context, TabItems);

            tabHeaderItems.Add(new TabItem(tabHeader,tabContent));

            output.SuppressOutput();
        }
        
        protected virtual string GetTabHeaderItem()
        {
            var id = TagHelper.Name + "-tab";
            var link = TagHelper.Name;
            var control = TagHelper.Name;
            var selected = TagHelper.Active ?? false;
            var active = selected?" active":"";
            var title = TagHelper.Title;

            return "<a class=\"nav-item nav-link"+ active + "\" id=\""+ id + "\" data-toggle=\""+ TabItemsDataTogglePlaceHolder+ "\" href=\"#"+ link + "\" role=\"tab\" aria-controls=\""+ control + "\" aria-selected=\""+ selected + "\">" +
                   title +
                   "</a>";
        }
        
        protected virtual string GetTabContentItem(string content)
        {
            var headerId = TagHelper.Name + "-tab";
            var id = TagHelper.Name;
            var selected = TagHelper.Active ?? false;
            var showActive = selected?" show active":"";

            return "<div class=\"tab-pane fade"+ showActive + "\" id=\""+ id + "\" role=\"tabpanel\" aria-labelledby=\""+ headerId + "\">" +
                   content +
                   "</div>";
        }
    }
}