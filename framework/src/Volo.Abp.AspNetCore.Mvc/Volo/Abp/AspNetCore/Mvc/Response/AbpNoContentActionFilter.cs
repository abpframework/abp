using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.AspNetCore.Filters;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Response;

public class AbpNoContentActionFilter : IAsyncActionFilter, IAbpFilter, ITransientDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }

        await next();

        if (!context.HttpContext.Response.HasStarted &&
            context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK &&
            context.Result == null)
        {
            var returnType = context.ActionDescriptor.GetReturnType();
            if (returnType == typeof(Task) || returnType == typeof(void))
            {
                if (context.HttpContext.Response.Body.CanSeek && context.HttpContext.Response.Body.Length != 0)
                {
                    // Body has been written, so we should not change the status code.
                    return;
                }

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
        }
    }
}
