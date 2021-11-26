using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets.Components.WidgetStyles;

public class WidgetStylesViewComponent : AbpViewComponent
{
    protected IPageWidgetManager PageWidgetManager { get; }
    protected AbpWidgetOptions Options { get; }

    public WidgetStylesViewComponent(
        IPageWidgetManager pageWidgetManager,
        IOptions<AbpWidgetOptions> options)
    {
        PageWidgetManager = pageWidgetManager;
        Options = options.Value;
    }

    public virtual IViewComponentResult Invoke()
    {
        return View(
            "~/Volo/Abp/AspNetCore/Mvc/UI/Widgets/Components/WidgetStyles/Default.cshtml",
            new WidgetResourcesViewModel
            {
                Widgets = PageWidgetManager.GetAll()
            }
        );
    }
}
