using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Volo.Abp.AspNetCore.Filters;

namespace Volo.Abp.AspNetCore.Middleware;

public abstract class AbpMiddlewareBase : IMiddleware
{
    protected Task<bool> ShouldSkipAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        var disableAbpFilterAttribute = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttribute<DisableAbpFilterAttribute>();
        return Task.FromResult(disableAbpFilterAttribute != null && disableAbpFilterAttribute.SkipInMiddleware);
    }

    public abstract Task InvokeAsync(HttpContext context, RequestDelegate next);
}
