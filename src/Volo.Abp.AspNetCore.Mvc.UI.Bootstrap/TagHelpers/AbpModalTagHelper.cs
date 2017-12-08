using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public class AbpModalTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //TODO: Remove abp-modal HTML element which is unnecessary!
            output.PreContent.SetHtmlContent(CreatePreContent());
            output.PostContent.SetHtmlContent(CreatePostContent());
        }

        private string CreatePreContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<div class=\"modal fade\" tabindex=\"-1\" role=\"dialog\" aria-hidden=\"true\">");
            sb.AppendLine("    <div class=\"modal-dialog\" role=\"document\">");
            sb.AppendLine("        <div class=\"modal-content\">");

            return sb.ToString();
        }

        private string CreatePostContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }
    }
}
