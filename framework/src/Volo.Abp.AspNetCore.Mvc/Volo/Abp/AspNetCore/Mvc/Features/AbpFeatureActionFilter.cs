using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.AspNetCore.Mvc.Features
{
    public class AbpFeatureActionFilter : IAsyncActionFilter, ITransientDependency
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            var methodInfo = context.ActionDescriptor.GetMethodInfo();

            using (AbpCrossCuttingConcerns.Applying(context.Controller, AbpCrossCuttingConcerns.FeatureChecking))
            {
                await context.HttpContext.RequestServices.GetRequiredService<IMethodInvocationFeatureCheckerService>().CheckAsync(
                    new MethodInvocationFeatureCheckerContext(methodInfo)
                );

                await next();
            }
        }
    }
}
