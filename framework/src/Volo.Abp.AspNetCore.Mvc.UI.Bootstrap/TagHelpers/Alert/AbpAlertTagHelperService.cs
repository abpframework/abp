using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Alert
{
    public class AbpAlertTagHelperService : AbpTagHelperService<AbpAlertTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            AddClasses(context, output);
            AddDismissButtonIfDismissible(context, output);
        }

        protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("role", "alert");
            output.Attributes.AddClass("alert");

            if (TagHelper.AlertType != AlertType.Default)
            {
                output.Attributes.AddClass("alert-" + TagHelper.AlertType.ToString().ToLowerInvariant());
            }

            if (TagHelper.Dismissible ?? false)
            {
                output.Attributes.AddClass("alert-dismissible");
                output.Attributes.AddClass("fade");
                output.Attributes.AddClass("show");
            }
        }

        protected virtual void AddDismissButtonIfDismissible(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.Dismissible ?? true)
            {
                return;
            }

            var buttonAsHtml =
                "<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>";

            output.PostContent.SetHtmlContent(buttonAsHtml);
        }
    }
}