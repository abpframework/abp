using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpRadioInputTagHelperService : AbpTagHelperService<AbpRadioInputTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var selectItems = GetSelectItems(context,output);

            var order = GetInputOrder(TagHelper.AspFor.ModelExplorer);

            var html = GetHtml(context, output, selectItems);


            AddGroupToFormGroupContents(context, TagHelper.AspFor.Name, html, order, out var surpress);

            if (surpress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "div";
                output.Attributes.Clear();
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetHtmlContent(html);
            }

        }

        protected virtual string GetHtml(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
        {
            var html = new StringBuilder("");

            foreach (var selectItem in selectItems)
            {
                var inlineClass = (TagHelper.Inline ?? false) ? " custom-control-inline" : "";
                var id = TagHelper.AspFor.Name + "Radio" + selectItem.Value;
                var name = TagHelper.AspFor.Name;
                var selected = selectItem.Selected ? " checked=\"checked\"" : "";

                var htmlPart = "<div class=\"custom-control custom-radio" + inlineClass + "\">\r\n" +
                               "  <input type=\"radio\" id=\"" + id + "\" name=\"" + name + "\" value=\"" + selectItem.Value + "\"" + selected + " class=\"custom-control-input\">\r\n" +
                               "  <label class=\"custom-control-label\" for=\"" + id + "\">" + selectItem.Text + "</label>\r\n" +
                               "</div>";

                html.AppendLine(htmlPart);
            }

            return html.ToString();
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
    }
}