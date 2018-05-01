using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public abstract class AbpButtonTagHelperServiceBase<TTagHelper> : AbpTagHelperService<TTagHelper>
        where TTagHelper : TagHelper, IButtonTagHelperBase
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            NormalizeTagStyle(context, output);
            AddClasses(context, output);
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

            if (TagHelper.Size != AbpButtonSize.Default)
            {
                output.Attributes.AddClass(TagHelper.Size.ToClassName());
            }
        }

        protected virtual void AddIcon(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Icon.IsNullOrWhiteSpace())
            {
                return;
            }

            output.Content.AppendHtml($"<i class=\"{GetIconClass(context, output)}\"></i> ");
        }

        protected virtual string GetIconClass(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.IconType)
            {
                case FontIconType.FontAwesome:
                    return "fa fa-" + TagHelper.Icon;
                default:
                    return TagHelper.Icon;
            }
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