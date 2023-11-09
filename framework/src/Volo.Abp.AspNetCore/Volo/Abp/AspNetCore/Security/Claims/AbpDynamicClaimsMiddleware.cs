using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Security.Claims;

public class AbpDynamicClaimsMiddleware : IMiddleware, ITransientDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var currentUser = context.RequestServices.GetRequiredService<ICurrentUser>();
        if (currentUser.IsAuthenticated)
        {
            var abpClaimsPrincipalFactory = context.RequestServices.GetRequiredService<IAbpClaimsPrincipalFactory>();
            context.User = await abpClaimsPrincipalFactory.CreateDynamicAsync(context.User);
        }

        await next(context);
    }
}
