using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets.Components.WidgetStyles
{
    public class WidgetStylesViewComponent : AbpViewComponent
    {
        protected IPageWidgetManager PageWidgetManager { get; }
        protected WidgetOptions Options { get; }

        public WidgetStylesViewComponent(
            IPageWidgetManager pageWidgetManager,
            IOptions<WidgetOptions> options)
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
}
