using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets.Components.WidgetStyles;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets.Components.WidgetScripts
{
    public class WidgetScriptsViewComponent : AbpViewComponent
    {
        protected IPageWidgetManager PageWidgetManager { get; }
        protected AbpWidgetOptions Options { get; }

        public WidgetScriptsViewComponent(
            IPageWidgetManager pageWidgetManager,
            IOptions<AbpWidgetOptions> options)
        {
            PageWidgetManager = pageWidgetManager;
            Options = options.Value;
        }

        public virtual IViewComponentResult Invoke()
        {
            return View(
                "~/Volo/Abp/AspNetCore/Mvc/UI/Widgets/Components/WidgetScripts/Default.cshtml",
                new WidgetResourcesViewModel
                {
                    Widgets = PageWidgetManager.GetAll()
                }
            );
        }
    }
}
