using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Components.LayoutHook;

public static class ViewComponentHelperLayoutHookExtensions
{
    public static Task<IHtmlContent> InvokeLayoutHookAsync(
        this IViewComponentHelper componentHelper,
        string name,
        string layout)
    {
        return componentHelper.InvokeAsync(
            typeof(LayoutHookViewComponent),
            new {
                name = name,
                layout = layout
            }
        );
    }
}
