using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabsTagHelperService : AbpTagHelperService<AbpTabsTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var items = InitilizeFormGroupContentsContext(context, output);

            await output.GetChildContentAsync();

            var headers = GetHeaders(context,output,items);
            var contents = GetConents(context,output,items);

            var surroundedHeaders = SurroundHeaders(context, output, headers);
            var surroundedContents = SurroundContents(context, output, contents);

            var finalContent = CombineHeadersAndContents(context, output, surroundedHeaders, surroundedContents);

            output.TagName = "div";
            output.Content.SetHtmlContent(finalContent);
            if (TagHelper.TabStyle == TabStyle.PillVertical)
            {
                PlaceInsideRow(output);
            }

        }



        protected virtual string CombineHeadersAndContents(TagHelperContext context, TagHelperOutput output, string headers, string contents)
        {
            var combined = new StringBuilder();

            if (TagHelper.TabStyle == TabStyle.PillVertical)
            {
                var headerColumnSize = GetHeaderColumnSize();
                var contentColumnSize = 12 - headerColumnSize;

                headers = PlaceInsideColunm(headers, headerColumnSize);
                contents = PlaceInsideColunm(contents, contentColumnSize);
            }


            combined.AppendLine(headers).AppendLine(contents);

            return combined.ToString();
        }



        protected virtual string MakeVerticalTab(TagHelperContext context, TagHelperOutput output, string headers, string contents)
        {


            return "";
        }

        protected virtual string SurroundHeaders(TagHelperContext context, TagHelperOutput output, string headers)
        {
            var id = TagHelper.Name;
            var navClass = TagHelper.TabStyle == TabStyle.Tab ? " nav-tabs" : " nav-pills";
            var verticalClass = GetVerticalPillClassIfVertical();

            var surroundedHeaders = "<nav>" + Environment.NewLine +
                                   "   <div class=\"nav"+ verticalClass + navClass + "\" id=\""+ id + "\" role=\"tablist\">" + Environment.NewLine +
                                   headers+
                                   "   </div>" + Environment.NewLine +
                                   "</nav>";

            return surroundedHeaders;
        }

        protected virtual string SurroundContents(TagHelperContext context, TagHelperOutput output, string contents)
        {
            var id = TagHelper.Name + "Content";

            var surroundedContents = "<div class=\"tab-content\" id=\""+ id + "\">" + Environment.NewLine +
                                   contents+
                                   "   </div>" ;

            return surroundedContents;
        }

        protected virtual string PlaceInsideColunm(string contents, int columnSize)
        {
            var surroundedContents = "<div class=\"col-"+ columnSize + "\">" + Environment.NewLine +
                                   contents+
                                   "   </div>" ;

            return surroundedContents;
        }

        protected virtual void PlaceInsideRow(TagHelperOutput output)
        {
            output.Attributes.AddClass("row");
        }

        protected virtual string GetHeaders(TagHelperContext context, TagHelperOutput output, List<TabItem> items)
        {
            var headersBuilder = new StringBuilder();

            foreach (var tabItem in items)
            {
                headersBuilder.AppendLine(tabItem.Header);
            }

            var headers = SetDataToggle(headersBuilder.ToString());

            return headers;
        }

        protected virtual string GetConents(TagHelperContext context, TagHelperOutput output, List<TabItem> items)
        {
            var contentsBuilder = new StringBuilder();

            foreach (var tabItem in items)
            {
                contentsBuilder.AppendLine(tabItem.Content);
            }

            return contentsBuilder.ToString();
        }

        protected virtual List<TabItem> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<TabItem>();
            context.Items[TabItems] = items;
            return items;
        }

        protected virtual string GetDataToggleStyle()
        {
            return TagHelper.TabStyle == TabStyle.Tab ? "tab" : "pill";
        }

        protected virtual string SetDataToggle(string content)
        {
            return content.Replace(TabItemsDataTogglePlaceHolder, GetDataToggleStyle());
        }

        protected virtual string GetVerticalPillClassIfVertical()
        {
            return TagHelper.TabStyle == TabStyle.PillVertical ? " flex-column " : "";
        }

        protected virtual int GetHeaderColumnSize()
        {
            return
                TagHelper.VerticalHeaderSize == ColumnSize.Undefined ||
                TagHelper.VerticalHeaderSize == ColumnSize.Auto || TagHelper.VerticalHeaderSize == ColumnSize._
                    ? (int)ColumnSize._3
                    : (int)TagHelper.VerticalHeaderSize;
        }
    }
}