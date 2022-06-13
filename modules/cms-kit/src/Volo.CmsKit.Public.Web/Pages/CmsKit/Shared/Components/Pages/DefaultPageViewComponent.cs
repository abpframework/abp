using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.HighlightJs;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Pages;

[Widget(
    StyleTypes = new[]
    {
        typeof(HighlightJsStyleContributor)
    },
    ScriptTypes = new[]
    {
        typeof(HighlightJsScriptContributor)
    },
    ScriptFiles = new[]
    {
        "/Pages/Public/CmsKit/highlightOnLoad.js"
    })]
public class DefaultPageViewComponent : AbpViewComponent //TODO: Remove component, directly render in the page
{
    public virtual async Task<IViewComponentResult> InvokeAsync(
        Guid pageId,
        string title,
        string content,
        List<ContentFragment> contentFragments)
    {
        var model = new PageViewModel
        {
            Id = pageId,
            Title = title,
            ContentFragments = contentFragments
        };

        return View("~/Pages/CmsKit/Shared/Components/Pages/Default.cshtml", model);
    }
}