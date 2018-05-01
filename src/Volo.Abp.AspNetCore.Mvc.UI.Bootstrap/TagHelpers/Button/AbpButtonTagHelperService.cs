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
            output.Attributes.Add("type", "button");
            output.Attributes.AddClass("btn");

            if (TagHelper.ButtonType != AbpButtonType.Default)
            {
                output.Attributes.AddClass("btn-" + TagHelper.ButtonType.ToString().ToLowerInvariant());
            }

            AddButtonBusyText(context, output);
        }

        protected virtual void AddButtonBusyText(TagHelperContext context, TagHelperOutput output)
        {
            if (output.Attributes.ContainsName(DataBusyTextAttributeName))
            {
                return;
            }

            if (string.Equals(output.Attributes["type"]?.Value.ToString(), "submit", StringComparison.OrdinalIgnoreCase))
            {
                output.Attributes.SetAttribute(DataBusyTextAttributeName, L["ProcessingWithThreeDot"]);
            }
        }
    }
}