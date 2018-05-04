using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown
{
    public class AbpDropdownButtonTagHelperService : AbpTagHelperService<AbpDropdownButtonTagHelper>
    {

        private readonly HtmlEncoder _htmlEncoder;
        private readonly IHtmlGenerator _htmlGenerator;
        private readonly IServiceProvider _serviceProvider;

        public AbpDropdownButtonTagHelperService(
            HtmlEncoder htmlEncoder,
            IHtmlGenerator htmlGenerator,
            IServiceProvider serviceProvider)
        {
            _htmlEncoder = htmlEncoder;
            _htmlGenerator = htmlGenerator;
            _serviceProvider = serviceProvider;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var buttonsAsHtml = GetButtonsAsHtml(context,output);

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


            var attributes = new TagHelperAttributeList();

            if (TagHelper.DropdownStyle == DropdownStyle.Single)
            {
                attributes.AddClass("dropdown-toggle");
                attributes.Add("data-toggle", "dropdown");
                attributes.Add("aria-haspopup", "true");
                attributes.Add("aria-expanded", "false");
            }

            if (output.Attributes.Any(at => at.Name == "href"))
            {
                attributes.Add("href", output.Attributes["href"].Value);
            }

            var mainButtonAsHtml = "";
            if (!TagHelper.Link??true)
            {
                mainButtonAsHtml = RenderTagHelper(attributes, context, abpButtonTagHelper, _htmlEncoder, "button", TagMode.StartTagAndEndTag);
            }
            else
            {
                var buttonTag = GetInnerTagHelper(attributes, context, abpButtonTagHelper, "button", TagMode.StartTagAndEndTag);
                var linkTag = ConvertButtonToLink(buttonTag);
                mainButtonAsHtml = RenderTagHelperOutput(linkTag, _htmlEncoder);
            }

            return mainButtonAsHtml;
        }

        protected virtual string GetSplitButton(TagHelperContext context, TagHelperOutput output)
        {
            var abpButtonTagHelper = _serviceProvider.GetRequiredService<AbpButtonTagHelper>();

            abpButtonTagHelper.Size = TagHelper.Size;
            abpButtonTagHelper.ButtonType = TagHelper.ButtonType;


            var attributes = new TagHelperAttributeList();
            
            attributes.AddClass("dropdown-toggle");
            attributes.AddClass("dropdown-toggle-split");
            attributes.Add("data-toggle", "dropdown");
            attributes.Add("aria-haspopup", "true");
            attributes.Add("aria-expanded", "false");

            var splitButtonAsHtml = "";
            if (true)
            {
                splitButtonAsHtml = RenderTagHelper(attributes, context, abpButtonTagHelper, _htmlEncoder, "button", TagMode.StartTagAndEndTag);
            }
            else
            {
                var buttonTag = GetInnerTagHelper(attributes, context, abpButtonTagHelper, "button", TagMode.StartTagAndEndTag);
                var linkTag = ConvertButtonToLink(buttonTag);
                splitButtonAsHtml = RenderTagHelperOutput(linkTag, _htmlEncoder);
            }

            return splitButtonAsHtml;
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