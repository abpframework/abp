using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalTagHelperService : AbpTagHelperService<AbpModalTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
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