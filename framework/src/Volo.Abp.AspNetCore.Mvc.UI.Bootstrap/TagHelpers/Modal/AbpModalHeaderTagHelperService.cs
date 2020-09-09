using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalHeaderTagHelperService : AbpTagHelperService<AbpModalHeaderTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-header");
            output.PreContent.SetHtmlContent(CreatePreContent());
            output.PostContent.SetHtmlContent(CreatePostContent());
        }

        protected virtual string CreatePreContent()
        {
            var title = new TagBuilder("h5");
            title.AddCssClass("modal-title");
            title.InnerHtml.Append(TagHelper.Title);

            return RenderHtml(title);
        }

        protected virtual string CreatePostContent()
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">");
            sb.AppendLine("        <span aria-hidden=\"true\">&times;</span>");
            sb.AppendLine("    </button>");

            return sb.ToString();
        }
    }
}