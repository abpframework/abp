using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Security.Claims;

public class AbpDynamicClaimsMiddleware : IMiddleware, ITransientDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            if (context.RequestServices.GetRequiredService<IOptions<AbpClaimsPrincipalFactoryOptions>>().Value.IsDynamicClaimsEnabled)
            {
                var abpClaimsPrincipalFactory = context.RequestServices.GetRequiredService<IAbpClaimsPrincipalFactory>();
                context.User = await abpClaimsPrincipalFactory.CreateDynamicAsync(context.User);
            }
        }

        await next(context);
    }
}
