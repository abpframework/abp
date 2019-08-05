using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Options;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public class WidgetPolicyChecker : IWidgetPolicyChecker
    {
        protected WidgetOptions Options { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected ICurrentUser CurrentUser { get; }

        public WidgetPolicyChecker(
            IOptions<WidgetOptions> widgetOptions,
            IAuthorizationService authorizationService,
            ICurrentUser currentUser)
        {
            AuthorizationService = authorizationService;
            CurrentUser = currentUser;
            Options = widgetOptions.Value;
        }

        public async Task<bool> CheckAsync(Type widgetComponentType)
        {
            var widget = Options.Widgets.Find(widgetComponentType);

            return await CheckAsyncInternal(widget, widgetComponentType.FullName);
        }

        public async Task<bool> CheckAsync(string name)
        {
            var widget = Options.Widgets.Find(name);

            return await CheckAsyncInternal(widget, name);
        }

        public async Task<bool> CheckAsyncInternal(WidgetDefinition widget, string wantedWidgetName)
        {
            if (widget == null)
            {
                throw new ArgumentNullException(wantedWidgetName);
            }

            if (widget.RequiredPolicies.Any())
            {
                foreach (var requiredPolicy in widget.RequiredPolicies)
                {
                    if (!(await AuthorizationService.AuthorizeAsync(requiredPolicy)).Succeeded)
                    {
                        return false;
                    }
                }
            }
            else if (widget.RequiresAuthentication && !CurrentUser.IsAuthenticated)
            {
                return false;
            }

            return true;
        }
    }
}
