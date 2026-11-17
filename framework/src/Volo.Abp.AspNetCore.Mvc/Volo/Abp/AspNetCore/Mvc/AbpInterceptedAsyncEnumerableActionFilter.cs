using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.AspNetCore.Filters;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.AspNetCore.Mvc;

/// <summary>
/// Waits for the interceptors of a returned <see cref="System.Collections.Generic.IAsyncEnumerable{T}"/>, so the outer action filters cover them.
/// </summary>
public class AbpInterceptedAsyncEnumerableActionFilter : IAsyncActionFilter, IAbpFilter, ITransientDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executedContext = await next();

        if ((executedContext.Exception == null || executedContext.ExceptionHandled) &&
            GetResultValue(executedContext.Result) is IAbpInterceptedAsyncEnumerable interceptedAsyncEnumerable)
        {
            await interceptedAsyncEnumerable.WaitForInterceptorsAsync();
        }
    }

    protected virtual object? GetResultValue(IActionResult? result)
    {
        return result switch
        {
            ObjectResult objectResult => objectResult.Value,
            JsonResult jsonResult => jsonResult.Value,
            _ => null
        };
    }
}
