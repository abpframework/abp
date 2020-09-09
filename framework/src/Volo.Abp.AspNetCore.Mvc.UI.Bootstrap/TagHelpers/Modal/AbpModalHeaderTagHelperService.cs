using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
{
    public class AbpModalHeaderTagHelperService : AbpTagHelperService<AbpModalHeaderTagHelper>
    {
        protected IStringLocalizer<AbpUiResource> L { get; }

        public AbpModalHeaderTagHelperService(IStringLocalizer<AbpUiResource> localizer)
        {
            L = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("modal-header");

            AddTitle(context, output);
            AddCloseButton(context, output);
        }

        protected virtual void AddTitle(TagHelperContext context, TagHelperOutput output)
        {
            var title = new TagBuilder("h5");
            title.AddCssClass("modal-title");
            title.InnerHtml.Append(TagHelper.Title);

            output.PreContent.SetHtmlContent(title);
        }

        protected virtual void AddCloseButton(TagHelperContext context, TagHelperOutput output)
        {
            var span = new TagBuilder("span");
            span.Attributes.Add("aria-hidden", "true");
            span.InnerHtml.AppendHtml("&times;");

            var button = new TagBuilder("button");
            button.AddCssClass("close");
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-dismiss", "modal");
            button.Attributes.Add("aria-label", L["Close"].Value);
            button.InnerHtml.AppendHtml(span);

            output.PostContent.AppendHtml(button);
        }
    }
}