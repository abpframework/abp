using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    [Dependency(ReplaceServices = true)]
    public class AbpViewComponentHelper : IViewComponentHelper, IViewContextAware, ITransientDependency
    {
        protected WidgetOptions Options { get; }
        protected IPageWidgetManager PageWidgetManager { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected ICurrentUser CurrentUser { get; }
        protected DefaultViewComponentHelper DefaultViewComponentHelper { get; }

        public AbpViewComponentHelper(
            DefaultViewComponentHelper defaultViewComponentHelper,
            IOptions<WidgetOptions> widgetOptions,
            IPageWidgetManager pageWidgetManager,
            IAuthorizationService authorizationService,
            ICurrentUser currentUser)
        {
            DefaultViewComponentHelper = defaultViewComponentHelper;
            PageWidgetManager = pageWidgetManager;
            AuthorizationService = authorizationService;
            CurrentUser = currentUser;
            Options = widgetOptions.Value;
        }

        public virtual async Task<IHtmlContent> InvokeAsync(string name, object arguments)
        {
            var widget = Options.Widgets.Find(name);
            if (widget == null)
            {
                return await DefaultViewComponentHelper.InvokeAsync(name, arguments);
            }

            return await InvokeWidgetAsync(arguments, widget);
        }

        public virtual async Task<IHtmlContent> InvokeAsync(Type componentType, object arguments)
        {
            var widget = Options.Widgets.Find(componentType);
            if (widget == null)
            {
                return await DefaultViewComponentHelper.InvokeAsync(componentType, arguments);
            }

            return await InvokeWidgetAsync(arguments, widget);
        }

        public virtual void Contextualize(ViewContext viewContext)
        {
            DefaultViewComponentHelper.Contextualize(viewContext);
        }

        protected virtual async Task<IHtmlContent> InvokeWidgetAsync(object arguments, WidgetDefinition widget)
        {
            if (widget.RequiredPolicies.Any())
            {
                foreach (var requiredPolicy in widget.RequiredPolicies)
                {
                    await AuthorizationService.AuthorizeAsync(requiredPolicy);
                }
            }
            else if (widget.RequiresAuthentication && !CurrentUser.IsAuthenticated)
            {
                throw new AbpAuthorizationException("Authorization failed! User has not logged in.");
            }

            PageWidgetManager.TryAdd(widget);

            var wrapperAttributesBuilder = new StringBuilder($"class=\"abp-widget-wrapper\" data-widget-name=\"{widget.Name}\"");

            if (widget.RefreshUrl != null)
            {
                wrapperAttributesBuilder.Append($" data-refresh-url=\"{widget.RefreshUrl}\"");
            }

            return new HtmlContentBuilder()
                .AppendHtml($"<div {wrapperAttributesBuilder}>")
                .AppendHtml(await DefaultViewComponentHelper.InvokeAsync(widget.ViewComponentType, arguments))
                .AppendHtml("</div>");
        }
    }
}