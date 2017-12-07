using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.TagHelpers
{
    public class AbpModalFooterTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "modal-footer"); //TODO: Append class if any exists
            output.Content.SetHtmlContent(CreateContent());
        }

        private string CreateContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">Cancel</button>");
            sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\" id=\"btnUpdateUserSave\">Save</button>");

            return sb.ToString();
        }
    }
}