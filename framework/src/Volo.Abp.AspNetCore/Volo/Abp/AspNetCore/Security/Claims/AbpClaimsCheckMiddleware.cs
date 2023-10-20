using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Security.Claims;

public class AbpClaimsCheckMiddleware : IMiddleware, ITransientDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var checkerTypes = context.RequestServices.GetRequiredService<IOptions<AbpClaimsCheckOptions>>().Value.Checker;
        foreach (var checkerType in checkerTypes)
        {
            var checker = context.RequestServices.GetRequiredService(checkerType) as IAbpClaimsChecker;

            var checkContext = new AbpClaimsCheckContext(context);
            await checker!.CheckAsync(checkContext);

            if (checkContext.CancelAuthentication)
            {
                context.User = null!;
                break;
            }
        }

        await next(context);
    }
}
