using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
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

        public AbpSelectTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder)
        {
            _generator = generator;
            _encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var innerHtml = GetFormInputGroupAsHtml(context, output);

            var order = GetInputOrder(TagHelper.AspFor.ModelExplorer);

            AddGroupToFormGroupContents(context, TagHelper.AspFor.Name, SurroundInnerHtmlAndGet(context, output, innerHtml), order, out var surpress);

            if (surpress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "div";
                output.Attributes.AddClass("form-group");
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetHtmlContent(innerHtml);
            }
        }

        protected virtual string GetFormInputGroupAsHtml(TagHelperContext context, TagHelperOutput output)
        {
            var selectTag = GetSelectTag(context, output);
            var selectAsHtml = RenderTagHelperOutput(selectTag, _encoder);
            var label = GetLabelAsHtml(context, output, selectTag);

            return label + Environment.NewLine + selectAsHtml;
        }

        protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml)
        {
            return "<div class=\"form-group\">" + Environment.NewLine + innerHtml + Environment.NewLine + "</div>";
        }

        protected virtual TagHelperOutput GetSelectTag(TagHelperContext context, TagHelperOutput output)
        {
            var selectItems = GetSelectItems(context, output);

            var selectTagHelper = new SelectTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                Items = selectItems,
                ViewContext = TagHelper.ViewContext
            };

            var inputTagHelperOutput = GetInnerTagHelper(new TagHelperAttributeList(), context, selectTagHelper, "select", TagMode.StartTagAndEndTag);

            inputTagHelperOutput.Attributes.Add("class", "form-control");

            return inputTagHelperOutput;
        }

        protected virtual List<SelectListItem> GetSelectItems(TagHelperContext context, TagHelperOutput output)
        {
            var selectItems = TagHelper.AspItems?.ToList();

            if (TagHelper.AspItems == null &&
                !GetSelectItemsIfProvidedByEnum(context, output, TagHelper.AspFor.ModelExplorer, out selectItems) &&
                !GetSelectItemsIfProvidedFromAttribute(context, output, TagHelper.AspFor.ModelExplorer, out selectItems))
            {
                throw new Exception("No items provided for select attribute.");
            }

            SetSelectedValue(context, output, selectItems);

            return selectItems;
        }

        protected virtual string GetLabelAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput selectTag)
        {
            if (string.IsNullOrEmpty(TagHelper.Label))
            {
                return GetLabelAsHtmlUsingTagHelper(context, output);
            }

            return "<label " + GetIdAttributeAsString(selectTag) + ">" + TagHelper.Label + "</label>";
        }

        protected virtual bool GetSelectItemsIfProvidedByEnum(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer, out List<SelectListItem> selectItems)
        {
            selectItems = explorer.Metadata.IsEnum ? explorer.ModelType.GetTypeInfo().GetMembers(BindingFlags.Public | BindingFlags.Static)
                .Select((t, i) => new SelectListItem { Value = i.ToString(), Text = t.Name }).ToList() : null;

            return selectItems != null;
        }

        protected virtual bool GetSelectItemsIfProvidedFromAttribute(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer, out List<SelectListItem> selectItems)
        {
            selectItems = GetAttribute<SelectItems>(explorer)?.GetItems(explorer)?.ToList();

            return selectItems != null;
        }

        protected virtual void SetSelectedValue(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
        {
            var selectedValue = GetSelectedValue(context, output);

            if (!selectItems.Any(si => si.Selected))
            {
                var itemToBeSelected = selectItems.FirstOrDefault(si => si.Value.ToString() == selectedValue);

                if (itemToBeSelected != null)
                {
                    itemToBeSelected.Selected = true;
                }
            }
        }

        protected virtual string GetSelectedValue(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.AspFor.ModelExplorer.Metadata.IsEnum)
            {
                var baseType = TagHelper.AspFor.ModelExplorer.Model?.GetType().GetEnumUnderlyingType();

                if (baseType == null)
                {
                    return null;
                }

                var valueAsString = Convert.ChangeType(TagHelper.AspFor.ModelExplorer.Model, baseType);
                return valueAsString != null ? valueAsString.ToString() : "";
            }

            return TagHelper.AspFor.ModelExplorer.Model?.ToString();
        }

        protected virtual string GetLabelAsHtmlUsingTagHelper(TagHelperContext context, TagHelperOutput output)
        {
            var labelTagHelper = new LabelTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };
            
            return RenderTagHelper(new TagHelperAttributeList(), context, labelTagHelper, _encoder, "span", TagMode.StartTagAndEndTag, true);
        }
    }
}