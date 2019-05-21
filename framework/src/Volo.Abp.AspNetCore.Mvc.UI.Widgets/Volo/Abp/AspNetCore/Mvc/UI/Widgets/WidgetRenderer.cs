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

        public async Task<IHtmlContent> RenderAsync(IViewComponentHelper component, string mywidget)
        {
            var componentType = _widgetOptions.Widgets.Single(w=>w.Name.Equals(mywidget)).ViewComponentType; 
            var args = new object();

            return await component.InvokeAsync(componentType, args);
        }
    }
}