﻿using System;
using System.Text;
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
        protected AbpWidgetOptions Options { get; }
        protected IPageWidgetManager PageWidgetManager { get; }
        protected DefaultViewComponentHelper DefaultViewComponentHelper { get; }

        public AbpViewComponentHelper(
            DefaultViewComponentHelper defaultViewComponentHelper,
            IOptions<AbpWidgetOptions> widgetOptions,
            IPageWidgetManager pageWidgetManager)
        {
            DefaultViewComponentHelper = defaultViewComponentHelper;
            PageWidgetManager = pageWidgetManager;
            Options = widgetOptions.Value;
        }

        public virtual async Task<IHtmlContent> InvokeAsync(string name, object arguments)
        {
            var widget = Options.Widgets.Find(name);
            if (widget == null)
            {
                return await DefaultViewComponentHelper.InvokeAsync(name, arguments).ConfigureAwait(false);
            }

            return await InvokeWidgetAsync(arguments, widget).ConfigureAwait(false);
        }

        public virtual async Task<IHtmlContent> InvokeAsync(Type componentType, object arguments)
        {
            var widget = Options.Widgets.Find(componentType);
            if (widget == null)
            {
                return await DefaultViewComponentHelper.InvokeAsync(componentType, arguments).ConfigureAwait(false);
            }

            return await InvokeWidgetAsync(arguments, widget).ConfigureAwait(false);
        }

        public virtual void Contextualize(ViewContext viewContext)
        {
            DefaultViewComponentHelper.Contextualize(viewContext);
        }

        protected virtual async Task<IHtmlContent> InvokeWidgetAsync(object arguments, WidgetDefinition widget)
        {
            PageWidgetManager.TryAdd(widget);

            var wrapperAttributesBuilder = new StringBuilder($"class=\"abp-widget-wrapper\" data-widget-name=\"{widget.Name}\"");

            if (widget.RefreshUrl != null)
            {
                wrapperAttributesBuilder.Append($" data-refresh-url=\"{widget.RefreshUrl}\"");
            }
            
            return new HtmlContentBuilder()
                .AppendHtml($"<div {wrapperAttributesBuilder}>")
                .AppendHtml(await DefaultViewComponentHelper.InvokeAsync(widget.ViewComponentType, arguments).ConfigureAwait(false))
                .AppendHtml("</div>");
        }
    }
}