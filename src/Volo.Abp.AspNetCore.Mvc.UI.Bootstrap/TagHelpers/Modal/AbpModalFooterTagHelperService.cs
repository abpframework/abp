using System.Text;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalFooterTagHelperService : AbpTagHelperService<AbpModalFooterTagHelper>
    {
        private readonly IStringLocalizer<AbpUiResource> _localizer;

        public AbpModalFooterTagHelperService(IStringLocalizer<AbpUiResource> localizer)
        {
            _localizer = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-footer");

            if (TagHelper.Buttons != AbpModalButtons.None)
            {
                output.PostContent.SetHtmlContent(CreateContent());
            }
        }

        protected virtual string CreateContent()
        {
            var sb = new StringBuilder();

            switch (TagHelper.Buttons)
            {
                case AbpModalButtons.Cancel:
                    sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + _localizer["Cancel"] + "</button>");
                    break;
                case AbpModalButtons.Close:
                    sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + _localizer["Close"] + "</button>");
                    break;
                case AbpModalButtons.Save:
                    sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\" data-busy-text=\"" + _localizer["SavingWithThreeDot"] + "\"><i class=\"fa fa-check\"></i> <span>" + _localizer["Save"] + "</span></button>");
                    break;
                case AbpModalButtons.Save | AbpModalButtons.Cancel:
                    sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + _localizer["Cancel"] + "</button>");
                    sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\" data-busy-text=\"" + _localizer["SavingWithThreeDot"] + "\"><i class=\"fa fa-check\"></i> <span>" + _localizer["Save"] + "</span></button>");
                    break;
                case AbpModalButtons.Save | AbpModalButtons.Close:
                    sb.AppendLine("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">" + _localizer["Close"] + "</button>");
                    sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\" data-busy-text=\"" + _localizer["SavingWithThreeDot"] + "\"><i class=\"fa fa-check\"></i> <span>" + _localizer["Save"] + "</span></button>");
                    break;
            }
            
            return sb.ToString();
        }
    }
}