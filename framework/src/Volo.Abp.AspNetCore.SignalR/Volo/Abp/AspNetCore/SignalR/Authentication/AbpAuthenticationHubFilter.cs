using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.SignalR.Authentication
{
    public class AbpAuthenticationHubFilter : IHubFilter
    {
        public virtual async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            var currentPrincipalAccessor = invocationContext.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            using (currentPrincipalAccessor.Change(invocationContext.Context.User))
            {
                return await next(invocationContext);
            }
        }

        public virtual async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            using (currentPrincipalAccessor.Change(context.Context.User))
            {
                await next(context);
            }
        }

        public virtual async Task OnDisconnectedAsync(HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next)
        {
            var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
            using (currentPrincipalAccessor.Change(context.Context.User))
            {
                await next(context, exception);
            }
        }
    }
}
