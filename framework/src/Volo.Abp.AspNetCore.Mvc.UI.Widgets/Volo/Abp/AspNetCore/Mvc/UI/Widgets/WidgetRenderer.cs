﻿using System.Linq;
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
            var widget = _widgetOptions.Widgets.Single(w=>w.Name.Equals(widgetName));

            return await componentHelper.InvokeAsync(widget.ViewComponentType, args ?? new object());
        }
    }
}