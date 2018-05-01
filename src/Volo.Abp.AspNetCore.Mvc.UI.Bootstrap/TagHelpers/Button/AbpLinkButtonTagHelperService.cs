using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public class AbpLinkButtonTagHelperService : AbpTagHelperService<AbpLinkButtonTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            NormalizeTagStyle(context, output);
            AddClasses(context, output);
            AddRole(context, output);
            AddType(context, output);
            AddIcon(context, output);
            AddText(context, output);
        }

        protected virtual void NormalizeTagStyle(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("btn");

            if (TagHelper.ButtonType != AbpButtonType.Default)
            {
                output.Attributes.AddClass("btn-" + TagHelper.ButtonType.ToString().ToLowerInvariant());
            }
        }

        protected virtual void AddType(TagHelperContext context, TagHelperOutput output)
        {
            if (!output.Attributes.ContainsName("type") &&
                output.TagName.Equals("input", StringComparison.InvariantCultureIgnoreCase))
            {
                output.Attributes.Add("type", "button");
            }
        }

        protected virtual void AddRole(TagHelperContext context, TagHelperOutput output)
        {
            if (!output.Attributes.ContainsName("role"))
            {
                output.Attributes.Add("role", "button");
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
    }
}