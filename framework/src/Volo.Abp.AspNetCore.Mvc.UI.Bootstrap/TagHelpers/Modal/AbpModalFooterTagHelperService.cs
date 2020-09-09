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
                    AddCancelButton(context, output);
                    break;
                case AbpModalButtons.Close:
                    AddCloseButton(context, output);
                    break;
                case AbpModalButtons.Save:
                    AddSaveButton(context, output);
                    break;
                case AbpModalButtons.Save | AbpModalButtons.Cancel:
                    AddSaveButton(context, output);
                    AddCancelButton(context, output);
                    break;
                case AbpModalButtons.Save | AbpModalButtons.Close:
                    AddSaveButton(context, output);
                    AddCloseButton(context, output);
                    break;
            }
        }

        protected virtual void AddSaveButton(TagHelperContext context, TagHelperOutput output) 
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

            output.PostContent.AppendHtml(button);
        }

        protected virtual void AddCloseButton(TagHelperContext context, TagHelperOutput output) 
        {
            var button = new TagBuilder("button");
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-dismiss", "modal");
            button.AddCssClass("btn");
            button.AddCssClass("btn-secondary");
            button.InnerHtml.Append(_localizer["Close"]);

            output.PostContent.AppendHtml(button);
        }

        protected virtual void AddCancelButton(TagHelperContext context, TagHelperOutput output) 
        {
            var button = new TagBuilder("button");
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-dismiss", "modal");
            button.AddCssClass("btn");
            button.AddCssClass("btn-secondary");
            button.InnerHtml.Append(_localizer["Cancel"]);

            output.PostContent.AppendHtml(button);
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