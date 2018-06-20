using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalHeaderTagHelperService : AbpTagHelperService<AbpModalHeaderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-header");
            output.Content.SetHtmlContent(CreateContent());
        }

        protected virtual string CreateContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("    <h5 class=\"modal-title\">" + TagHelper.Title + "</h5>");
            sb.AppendLine("    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">");
            sb.AppendLine("        <span aria-hidden=\"true\">&times;</span>");
            sb.AppendLine("    </button>");

            return sb.ToString();
        }
    }
}