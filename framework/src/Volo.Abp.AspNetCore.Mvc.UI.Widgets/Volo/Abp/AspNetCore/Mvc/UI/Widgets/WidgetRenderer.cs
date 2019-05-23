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

        public WidgetRenderer(IOptions<WidgetOptions> widgetOptions)
        {
            _widgetOptions = widgetOptions.Value;
        }

        public async Task<IHtmlContent> RenderAsync(IViewComponentHelper componentHelper, string widgetName, object args = null)
        {
            var componentType = _widgetOptions.Widgets.Single(w=>w.Name.Equals(widgetName)).ViewComponentType;

            return await componentHelper.InvokeAsync(componentType, args ?? new object());
        }
    }
}