using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.HighlightJs;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

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
public class DefaultPageViewComponent : AbpViewComponent
{
    private readonly IContentParser _contentParser;

    public DefaultPageViewComponent(IContentParser contentParser)
    {
        _contentParser = contentParser;
    }
    
    public virtual async Task<IViewComponentResult> InvokeAsync(
        Guid pageId,
        string title,
        string content)
    {
        var contentFragments = await _contentParser.ParseAsync(content);
        
        var model = new PageViewModel
        {
            Id = pageId,
            Title = title,
            ContentFragments = contentFragments
        };

        return View("~/Pages/CmsKit/Shared/Components/Pages/Default.cshtml", model);
    }
}