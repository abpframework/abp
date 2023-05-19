using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.AspNetCore.SignalR;

public class AbpHubContextAccessorHubFilter : IHubFilter
{
    public virtual async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
    {
        var hubContextAccessor = invocationContext.ServiceProvider.GetRequiredService<IAbpHubContextAccessor>();
        using (hubContextAccessor.Change(new AbpHubContext(
                   invocationContext.ServiceProvider,
                   invocationContext.Hub,
                   invocationContext.HubMethod,
                   invocationContext.HubMethodArguments)))
        {
            return await next(invocationContext);
        }
    }
}
