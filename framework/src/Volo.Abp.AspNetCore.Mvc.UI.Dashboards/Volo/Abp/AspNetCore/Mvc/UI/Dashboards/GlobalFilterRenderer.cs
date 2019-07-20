using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class GlobalFilterRenderer : IGlobalFilterRenderer
    {
        private readonly GlobalFilterOptions _globalFilterOptions;

        public GlobalFilterRenderer(IOptions<GlobalFilterOptions> widgetOptions)
        {
            _globalFilterOptions = widgetOptions.Value;
        }

        public async Task<IHtmlContent> RenderAsync(IViewComponentHelper componentHelper, string globalFilterName, object args = null)
        {
            var globalFilter = _globalFilterOptions.GlobalFilters.Single(w => w.Name.Equals(globalFilterName));

            return await componentHelper.InvokeAsync(globalFilter.ViewComponentType, args ?? new object());
        }
    }
}
