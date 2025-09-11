using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab;

public class AbpTabsTagHelperService : AbpTagHelperService<AbpTabsTagHelper>
{
    protected IHtmlGenerator HtmlGenerator { get; }

    public AbpTabsTagHelperService(IHtmlGenerator htmlGenerator)
    {
        HtmlGenerator = htmlGenerator;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        SetRandomNameIfNotProvided();

        var items = InitilizeFormGroupContentsContext(context, output);

        await output.GetChildContentAsync();

        var headers = GetHeaders(context, output, items);
        var contents = GetConents(context, output, items);

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

            headers = PlaceInsideColumn(headers, headerColumnSize);
            contents = PlaceInsideColumn(contents, contentColumnSize);
        }


        combined.AppendLine(headers).AppendLine(contents);

        return combined.ToString();
    }

    protected virtual string SurroundHeaders(TagHelperContext context, TagHelperOutput output, string headers)
    {
        var id = TagHelper.Name;
        var navClass = TagHelper.TabStyle == TabStyle.Tab ? " nav-tabs" : " nav-pills";
        var verticalClass = GetVerticalPillClassIfVertical();

        var listElement = new TagBuilder("ul");
        listElement.AddCssClass("nav" + verticalClass + navClass);
        listElement.Attributes.Add("id", id);
        listElement.Attributes.Add("role", "tablist");
        listElement.InnerHtml.AppendHtml(headers);

        return listElement.ToHtmlString();
    }

    protected virtual string SurroundContents(TagHelperContext context, TagHelperOutput output, string contents)
    {
        var id = TagHelper.Name + "Content";

        var wrapper = new TagBuilder("div");
        wrapper.AddCssClass("tab-content");
        wrapper.Attributes.Add("id", id);
        wrapper.InnerHtml.AppendHtml(contents);

        return wrapper.ToHtmlString();
    }

    protected virtual string PlaceInsideColumn(string contents, int columnSize)
    {
        var wrapper = new TagBuilder("div");
        wrapper.AddCssClass("col-md-" + columnSize);
        wrapper.InnerHtml.AppendHtml(contents);

        return wrapper.ToHtmlString();
    }

    protected virtual void PlaceInsideRow(TagHelperOutput output)
    {
        output.Attributes.AddClass("row");
    }

    protected virtual void SetActiveTab(List<TabItem> items)
    {
        if (!items.Any(it => it.Active) && items.Count > 0)
        {
            var firstItem = items.FirstOrDefault(i => !i.IsDropdown);
            if (firstItem != null)
            {
                firstItem.Active = true;
            }
        }

        foreach (var tabItem in items)
        {
            if (tabItem.Active)
            {
                tabItem.Content = tabItem.Content.Replace(AbpTabItemShowActivePlaceholder, " show active");
                tabItem.Header = tabItem.Header.Replace(AbpTabItemActivePlaceholder, " active").Replace(AbpTabItemSelectedPlaceholder, "true");
            }
            else
            {
                tabItem.Content = tabItem.Content.Replace(AbpTabItemShowActivePlaceholder, "");
                tabItem.Header = tabItem.Header.Replace(AbpTabItemActivePlaceholder, "").Replace(AbpTabItemSelectedPlaceholder, "false");
            }
        }

    }

    protected virtual string GetHeaders(TagHelperContext context, TagHelperOutput output, List<TabItem> items)
    {
        SetActiveTab(items);

        var headersBuilder = new StringBuilder();

        for (var index = 0; index < items.Count; index++)
        {
            var item = items[index];
            var header = "";
            if (item.IsDropdown)
            {
                var childHeaders = items.Where(i => i.ParentId == item.Id).Select(c => SetTabItemNameIfNotProvided(c.Header, items.IndexOf(c)));
                var childHeadersAsString = string.Join(Environment.NewLine, childHeaders.ToArray());
                header = item.Header.Replace(AbpTabDropdownItemsActivePlaceholder, childHeadersAsString);
            }
            else if (string.IsNullOrWhiteSpace(item.ParentId))
            {
                header = item.Header;

                header = SetTabItemNameIfNotProvided(header, index);
            }

            headersBuilder.AppendLine(header);
        }

        var headers = SetDataToggle(headersBuilder.ToString());

        return headers;
    }

    protected virtual string GetConents(TagHelperContext context, TagHelperOutput output, List<TabItem> items)
    {
        var contentsBuilder = new StringBuilder();

        for (var index = 0; index < items.Count; index++)
        {
            if (items[index].IsDropdown)
            {
                continue;
            }

            var content = items[index].Content;

            content = SetTabItemNameIfNotProvided(content, index);

            contentsBuilder.AppendLine(content);
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

    protected virtual void SetRandomNameIfNotProvided()
    {
        if (string.IsNullOrWhiteSpace(TagHelper.Name))
        {
            TagHelper.Name = "T" + Guid.NewGuid().ToString("N");
        }
    }

    protected virtual string SetTabItemNameIfNotProvided(string content, int index)
    {
        return content.Replace(TabItemNamePlaceHolder, HtmlGenerator.Encode(TagHelper.Name) + "_" + index);
    }
}
