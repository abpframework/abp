using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpSelectTagHelperService : AbpTagHelperService<AbpSelectTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;
        private readonly IStringLocalizer<AbpUiResource> _localizer;

        public AbpSelectTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IStringLocalizer<AbpUiResource> localizer)
        {
            _generator = generator;
            _encoder = encoder;
            _localizer = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            SetDivAttributes(output);
            output.TagMode = TagMode.StartTagAndEndTag;

            var selectTag = GetSelectTag();

            var seelctAsHtml = RenderTagHelperOutput(selectTag, _encoder);
            var labelAsHtml = GetLabelAsHtml(selectTag);
            var content = labelAsHtml + Environment.NewLine + seelctAsHtml;

            output.Content.SetHtmlContent(content);
        }

        protected virtual void SetDivAttributes(TagHelperOutput output)
        {
            output.Attributes.RemoveAll("asp-for");
            output.Attributes.RemoveAll("asp-items");
            output.Attributes.Add("class", "form-group");
        }

        protected virtual TagHelperOutput GetSelectTag()
        {
            var selectTagHelper = new SelectTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                Items = TagHelper.AspItems,
                ViewContext = TagHelper.ViewContext
            };

            var attributes = new TagHelperAttributeList { new TagHelperAttribute("class", "form-control") };
            var inputTagHelperOutput = GetInnerTagHelper(attributes, selectTagHelper, "select", TagMode.StartTagAndEndTag); ;

            inputTagHelperOutput.Attributes.Add("class", "form-control");

            return inputTagHelperOutput;
        }


        protected virtual string GetLabelAsHtml(TagHelperOutput selectTag)
        {
            if (string.IsNullOrEmpty(TagHelper.Label) && string.IsNullOrEmpty(TagHelper.AspFor.Metadata.DisplayName))
            {
                return "";
            }

            var label = GetLabelText();
            var idAttr = selectTag.Attributes.FirstOrDefault(a => a.Name == "id");
            var idAttrAsString = "";

            if (idAttr != null)
            {
                idAttrAsString = "for=\"" + idAttr.Value + "\"";
            }

            return "<label " + idAttrAsString + ">" + _localizer[label] + "</label>";
        }

        protected virtual string GetLabelText()
        {
            return string.IsNullOrEmpty(TagHelper.Label) ?
                TagHelper.AspFor.Metadata.DisplayName :
                TagHelper.Label;
        }
    }
}