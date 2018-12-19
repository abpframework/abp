using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
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

            inputTagHelperOutput.Attributes.AddClass("form-control");
            inputTagHelperOutput.Attributes.AddClass(GetSize(context,output));
            AddDisabledAttribute(inputTagHelperOutput);

            return inputTagHelperOutput;
        }

        protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
        {
            var disabledAttribute = GetAttribute<DisabledInput>(TagHelper.AspFor.ModelExplorer);

            if (disabledAttribute != null && !inputTagHelperOutput.Attributes.ContainsName("disabled"))
            {
                inputTagHelperOutput.Attributes.Add("disabled", "");
            }
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
            if (!string.IsNullOrEmpty(TagHelper.Label))
            {
                return "<label " + GetIdAttributeAsString(selectTag) + ">" + TagHelper.Label + "</label>";
            }

            return GetLabelAsHtmlUsingTagHelper(context, output);
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
            if (!selectItems.Any(si => si.Selected))
            {
                var selectedValue = GetSelectedValue(context, output);

                var itemToBeSelected = selectItems.FirstOrDefault(si => si.Value.ToString() == selectedValue);

                if (itemToBeSelected != null)
                {
                    itemToBeSelected.Selected = true;
                }
            }
        }

        protected virtual string GetSelectedValue(TagHelperContext context, TagHelperOutput output)
        {
            var modelExplorer = TagHelper.AspFor.ModelExplorer;

            if (modelExplorer.Metadata.IsEnum)
            {
                var baseType = modelExplorer.Model?.GetType().GetEnumUnderlyingType();

                if (baseType == null) { return null; }

                return Convert.ChangeType(modelExplorer.Model, baseType)?.ToString() ?? "";
            }
            else
            {
                return modelExplorer.Model?.ToString();
            }
        }

        protected virtual string GetLabelAsHtmlUsingTagHelper(TagHelperContext context, TagHelperOutput output)
        {
            var labelTagHelper = new LabelTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            return RenderTagHelper(new TagHelperAttributeList(), context, labelTagHelper, _encoder, "label", TagMode.StartTagAndEndTag, true);
        }

        protected virtual string GetSize(TagHelperContext context, TagHelperOutput output)
        {
            var attribute = GetAttribute<FormControlSize>(TagHelper.AspFor.ModelExplorer);

            if (attribute != null)
            {
                TagHelper.Size = attribute.Size;
            }

            switch (TagHelper.Size)
            {
                case AbpFormControlSize.Small:
                    return "form-control-sm";
                case AbpFormControlSize.Medium:
                    return "form-control-md";
                case AbpFormControlSize.Large:
                    return "form-control-lg";
            }

            return "";
        }
    }
}