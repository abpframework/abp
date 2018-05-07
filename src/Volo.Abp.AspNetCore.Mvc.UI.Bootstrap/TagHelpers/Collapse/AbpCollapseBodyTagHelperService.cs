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

            var innerContent = (await output.GetChildContentAsync()).GetContent();

            var body = GetBody(context, output, innerContent);

            output.Content.SetHtmlContent(body);
        }

        protected virtual string GetBody(TagHelperContext context, TagHelperOutput output, string innerContent)
        {
            return "<div class=\"card card-body\">" + innerContent + "</div>";
        }
    }
}