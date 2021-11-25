using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Breadcrumb;

public class AbpBreadcrumbTagHelperService : AbpTagHelperService<AbpBreadcrumbTagHelper>
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";
        output.Attributes.Add("aria-label", "breadcrumb");

        var list = InitilizeFormGroupContentsContext(context, output);

        await output.GetChildContentAsync();

        var listTagBuilder = GetOlTagBuilder();

        SetInnerList(context, output, list, listTagBuilder);

        output.Content.SetHtmlContent(listTagBuilder);
    }

    protected virtual TagBuilder GetOlTagBuilder()
    {
        var builder = new TagBuilder("ol");
        builder.AddCssClass("breadcrumb");
        return builder;
    }

    protected virtual void SetInnerList(TagHelperContext context, TagHelperOutput output, List<BreadcrumbItem> list, TagBuilder listTagBuilder)
    {
        SetLastOneActiveIfThereIsNotAny(context, output, list);

        foreach (var breadcrumbItem in list)
        {
            var htmlPart = SetActiveClassIfActiveAndGetHtml(breadcrumbItem);

            listTagBuilder.InnerHtml.AppendHtml(htmlPart);
        }
    }

    protected virtual List<BreadcrumbItem> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
    {
        var items = new List<BreadcrumbItem>();
        context.Items[BreadcrumbItemsContent] = items;
        return items;
    }

    protected virtual void SetLastOneActiveIfThereIsNotAny(TagHelperContext context, TagHelperOutput output, List<BreadcrumbItem> list)
    {
        if (list.Count > 0 && !list.Any(bc => bc.Active))
        {
            list.Last().Active = true;
        }
    }

    protected virtual string SetActiveClassIfActiveAndGetHtml(BreadcrumbItem item)
    {
        return item.Active ?
            item.Html.Replace(AbpBreadcrumbItemActivePlaceholder, " active") :
            item.Html.Replace(AbpBreadcrumbItemActivePlaceholder, "");
    }

}
