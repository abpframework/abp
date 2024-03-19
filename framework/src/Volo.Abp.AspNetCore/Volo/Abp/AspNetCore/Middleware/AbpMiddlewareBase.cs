using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Volo.Abp.AspNetCore.Middleware;

public abstract class AbpMiddlewareBase : IMiddleware
{
    protected virtual Task<bool> ShouldSkipAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        var disableAbpFeaturesAttribute = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttribute<DisableAbpFeaturesAttribute>();
        return Task.FromResult(disableAbpFeaturesAttribute != null && disableAbpFeaturesAttribute.DisableMiddleware);
    }

    public abstract Task InvokeAsync(HttpContext context, RequestDelegate next);
}
