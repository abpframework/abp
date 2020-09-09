using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                ProcessButtons(context, output);
            }

            ProcessButtonsAlignment(output);
        }

        protected virtual void ProcessButtons(TagHelperContext context, TagHelperOutput output) 
        {
            switch (TagHelper.Buttons) 
            {
                case AbpModalButtons.Cancel:
                    output.PostContent.AppendHtml(GetCancelButton());
                    break;
                case AbpModalButtons.Close:
                    output.PostContent.AppendHtml(GetCloseButton());
                    break;
                case AbpModalButtons.Save:
                    output.PostContent.AppendHtml(GetSaveButton());
                    break;
                case AbpModalButtons.Save | AbpModalButtons.Cancel:
                    output.PostContent.AppendHtml(GetSaveButton());
                    output.PostContent.AppendHtml(GetCancelButton());
                    break;
                case AbpModalButtons.Save | AbpModalButtons.Close:
                    output.PostContent.AppendHtml(GetSaveButton());
                    output.PostContent.AppendHtml(GetCloseButton());
                    break;
            }
        }

        protected virtual string GetSaveButton() 
        {
            var icon = new TagBuilder("i");
            icon.AddCssClass("fa");
            icon.AddCssClass("fa-check");

            var span = new TagBuilder("span");
            span.InnerHtml.Append(_localizer["Save"]);

            var button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.AddCssClass("btn");
            button.AddCssClass("btn-primary");
            button.Attributes.Add("data-busy-text", _localizer["SavingWithThreeDot"]);
            button.InnerHtml.AppendHtml(icon);
            button.InnerHtml.AppendHtml(span);

            return RenderHtml(button);
        }

        protected virtual string GetCloseButton() 
        {
            var button = new TagBuilder("button");
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-dismiss", "modal");
            button.AddCssClass("btn");
            button.AddCssClass("btn-secondary");
            button.InnerHtml.Append(_localizer["Close"]);

            return RenderHtml(button);
        }

        protected virtual string GetCancelButton() 
        {
            var button = new TagBuilder("button");
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-dismiss", "modal");
            button.AddCssClass("btn");
            button.AddCssClass("btn-secondary");
            button.InnerHtml.Append(_localizer["Cancel"]);

            return RenderHtml(button);
        }

        protected virtual void ProcessButtonsAlignment(TagHelperOutput output)
        {
            if (TagHelper.ButtonAlignment == ButtonsAlign.Default)
            {
                return;
            }
            output.Attributes.AddClass("justify-content-" + TagHelper.ButtonAlignment.ToString().ToLowerInvariant());
        }
    }
}