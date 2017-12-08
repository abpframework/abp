using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Localization.Resource;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public class AbpModalFooterTagHelper : TagHelper
    {
        private readonly IStringLocalizer<AbpBootstrapResource> _localizer;

        public AbpModalFooterTagHelper(IStringLocalizer<AbpBootstrapResource> localizer)
        {
            _localizer = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "modal-footer"); //TODO: Append class if any exists
            output.Content.SetHtmlContent(CreateContent());
        }

        private string CreateContent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + _localizer["Close"] + "</button>");
            sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\" id=\"btnUpdateUserSave\">" + _localizer["Save"] + "</button>");

            return sb.ToString();
        }
    }
}