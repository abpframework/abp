using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalHeaderTagHelper : TagHelper
    {
        public string Title { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-header");
            output.Content.SetHtmlContent(CreateContent());
        }

        private string CreateContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("    <h5 class=\"modal-title\">" + Title + "</h5>");
            sb.AppendLine("    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">");
            sb.AppendLine("        <span aria-hidden=\"true\">&times;</span>");
            sb.AppendLine("    </button>");

            return sb.ToString();
        }
    }
}