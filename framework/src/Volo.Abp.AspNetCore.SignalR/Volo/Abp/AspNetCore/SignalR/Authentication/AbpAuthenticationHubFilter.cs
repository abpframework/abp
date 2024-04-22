using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.SignalR.Authentication;

public class AbpAuthenticationHubFilter : IHubFilter
{
    public virtual async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
    {
        var currentPrincipalAccessor = invocationContext.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
        using (currentPrincipalAccessor.Change((await GetDynamicClaimsPrincipalAsync(invocationContext.Context.User, invocationContext.ServiceProvider))!))
        {
            return await next(invocationContext);
        }
    }

    public virtual async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
        using (currentPrincipalAccessor.Change((await GetDynamicClaimsPrincipalAsync(context.Context.User, context.ServiceProvider))!))
        {
            await next(context);
        }
    }

    public virtual async Task OnDisconnectedAsync(HubLifetimeContext context, Exception? exception, Func<HubLifetimeContext, Exception?, Task> next)
    {
        var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
        using (currentPrincipalAccessor.Change((await GetDynamicClaimsPrincipalAsync(context.Context.User, context.ServiceProvider))!))
        {
            await next(context, exception);
        }
    }

    protected virtual async Task<ClaimsPrincipal?> GetDynamicClaimsPrincipalAsync(ClaimsPrincipal? claimsPrincipal, IServiceProvider serviceProvider)
    {
        if (claimsPrincipal == null)
        {
            return claimsPrincipal;
        }

        if (claimsPrincipal.Identity != null &&
            claimsPrincipal.Identity.IsAuthenticated &&
            serviceProvider.GetRequiredService<IOptions<AbpClaimsPrincipalFactoryOptions>>().Value.IsDynamicClaimsEnabled)
        {
            var abpClaimsPrincipalFactory = serviceProvider.GetRequiredService<IAbpClaimsPrincipalFactory>();
            claimsPrincipal = await abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        }

        return claimsPrincipal;
    }
}
