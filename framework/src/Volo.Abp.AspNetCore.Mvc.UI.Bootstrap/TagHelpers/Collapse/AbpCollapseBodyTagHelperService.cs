using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{
    public class AbpCollapseBodyTagHelperService : AbpTagHelperService<AbpCollapseBodyTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("collapse");
            output.Attributes.Add("id", TagHelper.Id);

            if (TagHelper.Show ?? false)
            {
                output.Attributes.AddClass("show");
            }

            if (TagHelper.Multi ?? false)
            {
                output.Attributes.AddClass("multi-collapse");
            }

            var innerContent = (await output.GetChildContentAsync()).GetContent();

            output.Content.SetHtmlContent(innerContent);
        }
    }
}
