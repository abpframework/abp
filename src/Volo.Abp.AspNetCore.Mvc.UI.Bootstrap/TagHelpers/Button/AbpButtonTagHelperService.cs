using System;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public class AbpButtonTagHelperService : AbpTagHelperService<AbpButtonTagHelper>
    {
        protected const string DataBusyTextAttributeName = "data-busy-text";

        protected IStringLocalizer<AbpUiResource> L { get; }

        public AbpButtonTagHelperService(IStringLocalizer<AbpUiResource> localizer)
        {
            L = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            NormalizeTagStyle(context, output);
            AddType(context, output);
            AddClasses(context, output);
            AddIcon(context, output);
            AddText(context, output);
            AddBusyText(context, output);
        }

        protected virtual void NormalizeTagStyle(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        protected virtual void AddType(TagHelperContext context, TagHelperOutput output)
        {
            if (output.Attributes.ContainsName("type"))
            {
                return;
            }

            output.Attributes.Add("type", "button");
        }

        protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("btn");

            if (TagHelper.ButtonType != AbpButtonType.Default)
            {
                output.Attributes.AddClass("btn-" + TagHelper.ButtonType.ToString().ToLowerInvariant());
            }
        }

        protected virtual void AddIcon(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Icon.IsNullOrWhiteSpace())
            {
                return;
            }

            output.Content.AppendHtml($"<i class=\"fa fa-{TagHelper.Icon}\"></i> ");
        }

        protected virtual void AddText(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            output.Content.AppendHtml($"<span>{TagHelper.Text}</span>");
        }

        protected virtual void AddBusyText(TagHelperContext context, TagHelperOutput output)
        {
            var busyText = TagHelper.BusyText ?? L["ProcessingWithThreeDot"];
            if (busyText.IsNullOrWhiteSpace())
            {
                return;
            }

            output.Attributes.SetAttribute(DataBusyTextAttributeName, busyText);
        }
    }
}