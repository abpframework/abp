using System;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown
{
    public class AbpDropdownButtonTagHelperService : AbpTagHelperService<AbpDropdownButtonTagHelper>
    {

        private readonly HtmlEncoder _htmlEncoder;
        private readonly IServiceProvider _serviceProvider;

        public AbpDropdownButtonTagHelperService(
            HtmlEncoder htmlEncoder,
            IServiceProvider serviceProvider)
        {
            _htmlEncoder = htmlEncoder;
            _serviceProvider = serviceProvider;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var buttonsAsHtml = GetButtonsAsHtml(context, output);

            output.PreElement.SetHtmlContent(buttonsAsHtml);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
        }

        protected virtual string GetButtonsAsHtml(TagHelperContext context, TagHelperOutput output)
        {
            var buttonBuilder = new StringBuilder("");

            var mainButton = GetMainButton(context, output);

            buttonBuilder.AppendLine(mainButton);

            if (TagHelper.DropdownStyle == DropdownStyle.Split)
            {
                var splitButton = GetSplitButton(context, output);

                buttonBuilder.AppendLine(splitButton);
            }

            return buttonBuilder.ToString();
        }

        protected virtual string GetMainButton(TagHelperContext context, TagHelperOutput output)
        {
            var abpButtonTagHelper = _serviceProvider.GetRequiredService<AbpButtonTagHelper>();

            abpButtonTagHelper.Icon = TagHelper.Icon;
            abpButtonTagHelper.Text = TagHelper.Text;
            abpButtonTagHelper.IconType = TagHelper.IconType;
            abpButtonTagHelper.Size = TagHelper.Size;
            abpButtonTagHelper.ButtonType = TagHelper.ButtonType;
            var attributes = GetAttributesForMainButton(context, output);

            var buttonTag = GetInnerTagHelper(attributes, context, abpButtonTagHelper, "button", TagMode.StartTagAndEndTag);

            if (TagHelper.Link ?? false)
            {
                var linkTag = ConvertButtonToLink(buttonTag);
                return RenderTagHelperOutput(linkTag, _htmlEncoder);
            }

            return RenderTagHelperOutput(buttonTag, _htmlEncoder);
        }

        protected virtual string GetSplitButton(TagHelperContext context, TagHelperOutput output)
        {
            var abpButtonTagHelper = _serviceProvider.GetRequiredService<AbpButtonTagHelper>();

            abpButtonTagHelper.Size = TagHelper.Size;
            abpButtonTagHelper.ButtonType = TagHelper.ButtonType;
            var attributes = GetAttributesForSplitButton(context, output);

            return RenderTagHelper(attributes, context, abpButtonTagHelper, _htmlEncoder, "button", TagMode.StartTagAndEndTag);
        }

        protected virtual TagHelperAttributeList GetAttributesForMainButton(TagHelperContext context, TagHelperOutput output)
        {

            var attributes = new TagHelperAttributeList();

            foreach (var tagHelperAttribute in output.Attributes)
            {
                attributes.Add(tagHelperAttribute);
            }

            if (TagHelper.DropdownStyle != DropdownStyle.Split)
            {
                attributes.AddClass("dropdown-toggle");
                attributes.Add("data-toggle", "dropdown");
                attributes.Add("aria-haspopup", "true");
                attributes.Add("aria-expanded", "false");
            }

            return attributes;
        }

        protected virtual TagHelperAttributeList GetAttributesForSplitButton(TagHelperContext context, TagHelperOutput output)
        {
            var attributes = new TagHelperAttributeList
            {
                {"data-toggle", "dropdown"},
                {"aria-haspopup", "true"},
                {"aria-expanded", "false"},
            };
            
            attributes.AddClass("dropdown-toggle");
            attributes.AddClass("dropdown-toggle-split");

            return attributes;
        }

        protected virtual TagHelperOutput ConvertButtonToLink(TagHelperOutput buttonTag)
        {
            buttonTag.TagName = "a";
            buttonTag.Attributes.RemoveAll("type");
            buttonTag.Attributes.Add("roles", "button");
            return buttonTag;
        }
    }
}