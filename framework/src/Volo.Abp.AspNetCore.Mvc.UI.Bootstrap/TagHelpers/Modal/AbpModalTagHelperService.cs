using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalTagHelperService : AbpTagHelperService<AbpModalTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            output.PreContent.SetHtmlContent(CreatePreContent(output));
            output.PostContent.SetHtmlContent(CreatePostContent());
        }

        protected virtual string CreatePreContent(TagHelperOutput output)
        {
            var sb = new StringBuilder();

            var attritubutes = output.Attributes.Select(a => " " + a.Name + "=\"" + a.Value + "\" ").ToList();
            var attritubutesAsJoin = string.Join(" ", attritubutes.ToArray()); 

            sb.AppendLine("<div class=\""+ GetModalClasses() + "\" tabindex=\"-1\" role=\"dialog\" aria-hidden=\"true\" "+ attritubutesAsJoin + ">");
            sb.AppendLine("    <div class=\"" + GetModalDialogClasses() + "\" role=\"document\">");
            sb.AppendLine("        <div class=\"" + GetModalContentClasses() + "\">");

            return sb.ToString();
        }

        protected virtual string GetModalClasses()
        {
            return "modal fade";
        }

        protected virtual string GetModalDialogClasses()
        {
            var classNames = new StringBuilder("modal-dialog");

            if (TagHelper.Size != AbpModalSize.Default)
            {
                classNames.Append(" ");
                classNames.Append(TagHelper.Size.ToClassName());
            }

            return classNames.ToString();
        }

        protected virtual string GetModalContentClasses()
        {
            return "modal-content";
        }

        protected virtual string CreatePostContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }
    }
}