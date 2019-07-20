using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class WidgetRenderer : IWidgetRenderer
    {
        private readonly WidgetOptions _widgetOptions;
        private readonly IPageWidgetManager _pageWidgetManager;

        public WidgetRenderer(IOptions<WidgetOptions> widgetOptions, IPageWidgetManager pageWidgetManager)
        {
            _pageWidgetManager = pageWidgetManager;
            _widgetOptions = widgetOptions.Value;
        }

        public async Task<IHtmlContent> RenderAsync(IViewComponentHelper componentHelper, string widgetName, object args = null)
        {
            var widget = _widgetOptions.Widgets.Single(w=>w.Name.Equals(widgetName));
            _pageWidgetManager.TryAdd(widget);
            return await componentHelper.InvokeAsync(widget.ViewComponentType, args ?? new object());
        }
    }
}