using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public abstract class AbpTagHelperService<TTagHelper> : IAbpTagHelperService<TTagHelper>
        where TTagHelper : TagHelper
    {
        protected const string FormGroupContents = "FormGroupContents";
        protected const string DropdownButtonsAsHtml = "DropdownButtonsAsHtmlContent";
        protected const string TabItems = "TabItems";
        protected const string AccordionItems = "AccordionItems";
        protected const string BreadcrumbItemsContent = "BreadcrumbItemsContent";
        protected const string CarouselItemsContent = "CarouselItemsContent";
        protected const string TabItemsDataTogglePlaceHolder = "{{data_toggle}}";
        protected const string TabItemsVerticalPillPlaceHolder = "{{vertical_pill}}";
        protected const string TabItemNamePlaceHolder = "{{Tag_Name}}";
        protected const string AbpFormContentPlaceHolder = "{_AbpFormContentPlaceHolder_}";
        protected const string AbpTabItemActivePlaceholder = "{_Tab_Active_Placeholder_}";
        protected const string AbpTabItemShowActivePlaceholder = "{_Tab_Show_Active_Placeholder_}";
        protected const string AbpBreadcrumbItemActivePlaceholder = "{_Breadcrumb_Active_Placeholder_}";
        protected const string AbpCarouselItemActivePlaceholder = "{_CarouselItem_Active_Placeholder_}";
        protected const string AbpTabItemSelectedPlaceholder = "{_Tab_Selected_Placeholder_}";
        protected const string AbpAccordionParentIdPlaceholder = "{_{{Parent_Accordion_Id}}_}";

        public TTagHelper TagHelper { get; internal set; }

        public virtual int Order { get; }

        public virtual void Init(TagHelperContext context)
        {

        }

        public virtual void Process(TagHelperContext context, TagHelperOutput output)
        {

        }

        public virtual Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Process(context, output);
            return Task.CompletedTask;
        }

        protected virtual TagHelperOutput GetInnerTagHelper(TagHelperAttributeList attributeList, TagHelperContext context, TagHelper tagHelper, string tagName = "div", TagMode tagMode = TagMode.SelfClosing, bool runAsync = false)
        {
            var innerOutput = new TagHelperOutput(tagName, attributeList, (useCachedResult, encoder) => Task.Run<TagHelperContent>(() => new DefaultTagHelperContent()))
            {
                TagMode = tagMode
            };

            var innerContext = new TagHelperContext(attributeList, context.Items, Guid.NewGuid().ToString());

            if (runAsync)
            {
                AsyncHelper.RunSync(() => tagHelper.ProcessAsync(innerContext, innerOutput));
            }
            else
            {
                tagHelper.Process(innerContext, innerOutput);
            }

            return innerOutput;
        }

        protected virtual string RenderTagHelper(TagHelperAttributeList attributeList, TagHelperContext context, TagHelper tagHelper, HtmlEncoder htmlEncoder, string tagName = "div", TagMode tagMode = TagMode.SelfClosing, bool runAsync = false)
        {
            var innerOutput = GetInnerTagHelper(attributeList, context, tagHelper, tagName, tagMode, runAsync);

            return RenderTagHelperOutput(innerOutput, htmlEncoder);
        }

        protected virtual string RenderTagHelperOutput(TagHelperOutput output, HtmlEncoder htmlEncoder)
        {
            using (var writer = new StringWriter())
            {
                output.WriteTo(writer, htmlEncoder);
                return writer.ToString();
            }
        }

        protected virtual T GetAttribute<T>(ModelExplorer property) where T : Attribute
        {
            return property?.Metadata?.ContainerType?.GetTypeInfo()?.GetProperty(property.Metadata.PropertyName)?.GetCustomAttribute<T>();
        }

        protected virtual List<FormGroupItem> GetFormGroupContentsList(TagHelperContext context, out bool surpress)
        {
            var items = GetValueFromContext<List<FormGroupItem>>(context, FormGroupContents);
            surpress = items != null;

            return items ?? new List<FormGroupItem>();
        }

        protected virtual T GetValueFromContext<T>(TagHelperContext context, string key)
        {
            if (!context.Items.ContainsKey(key))
            {
                return default;
            }
            
            return (T)context.Items[key];
        }

        protected virtual string GetIdAttributeAsString(TagHelperOutput inputTag)
        {
            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

            return idAttr != null ? "for=\"" + idAttr.Value + "\"" : "";
        }

        protected virtual int GetInputOrder(ModelExplorer explorer)
        {
            return GetAttribute<DisplayOrder>(explorer)?.Number ?? DisplayOrder.Default;
        }

        protected virtual void AddGroupToFormGroupContents(TagHelperContext context, string propertyName, string html, int order, out bool surpress)
        {
            var list = GetFormGroupContentsList(context, out surpress);

            if (list != null && !list.Any(igc => igc.HtmlContent.Contains("id=\"" + propertyName.Replace('.', '_') + "\"")))
            {
                list.Add(new FormGroupItem
                {
                    HtmlContent = html,
                    Order = order
                });
            }
        }
    }
}