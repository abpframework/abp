using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    [Dependency(ReplaceServices = true)]
    public class AbpViewComponentHelper : IViewComponentHelper, IViewContextAware, ITransientDependency
    {
        protected WidgetOptions Options { get; }
        protected IPageWidgetManager PageWidgetManager { get; }
        protected DefaultViewComponentHelper DefaultViewComponentHelper { get; }

        public AbpViewComponentHelper(
            DefaultViewComponentHelper defaultViewComponentHelper,
            IOptions<WidgetOptions> widgetOptions,
            IPageWidgetManager pageWidgetManager)
        {
            DefaultViewComponentHelper = defaultViewComponentHelper;
            PageWidgetManager = pageWidgetManager;
            Options = widgetOptions.Value;
        }

        public Task<IHtmlContent> InvokeAsync(string name, object arguments)
        {
            var widget = Options.Widgets.FirstOrDefault(w => w.Name == name); //Optimize using a dictionary by name
            if (widget != null)
            {
                PageWidgetManager.TryAdd(widget);
            }

            return DefaultViewComponentHelper.InvokeAsync(name, arguments);
        }

        public Task<IHtmlContent> InvokeAsync(Type componentType, object arguments)
        {
            var widget = Options.Widgets.FirstOrDefault(w => w.ViewComponentType == componentType); //Optimize using a dictionary by type
            if (widget != null)
            {
                PageWidgetManager.TryAdd(widget);
            }

            return DefaultViewComponentHelper.InvokeAsync(componentType, arguments);
        }

        public void Contextualize(ViewContext viewContext)
        {
            DefaultViewComponentHelper.Contextualize(viewContext);
        }
    }
}