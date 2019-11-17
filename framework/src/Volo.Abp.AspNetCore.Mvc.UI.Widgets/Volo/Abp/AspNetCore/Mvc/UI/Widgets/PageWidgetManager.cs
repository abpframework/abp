using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class PageWidgetManager : IPageWidgetManager, IScopedDependency
    {
        public const string HttpContextItemName = "__AbpCurrentWidgets";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PageWidgetManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool TryAdd(WidgetDefinition widget)
        {
            return GetWidgetList()
                .AddIfNotContains(widget);
        }

        private List<WidgetDefinition> GetWidgetList()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new AbpException($"{typeof(PageWidgetManager).AssemblyQualifiedName} should be used in a web request! Can not get IHttpContextAccessor.HttpContext.");
            }

            var widgets = httpContext.Items[HttpContextItemName] as List<WidgetDefinition>;
            if (widgets == null)
            {
                widgets = new List<WidgetDefinition>();
                httpContext.Items[HttpContextItemName] = widgets;
            }

            return widgets;
        }

        public IReadOnlyList<WidgetDefinition> GetAll()
        {
            return GetWidgetList().ToImmutableArray();
        }
    }
}