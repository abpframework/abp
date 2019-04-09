using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.AspNetCore.Mvc.Features
{
    public class AbpFeatureActionFilter : IAsyncActionFilter, ITransientDependency
    {
        private readonly IMethodInvocationFeatureCheckerService _methodInvocationAuthorizationService;

        public AbpFeatureActionFilter(IMethodInvocationFeatureCheckerService methodInvocationAuthorizationService)
        {
            _methodInvocationAuthorizationService = methodInvocationAuthorizationService;
        }

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
                await _methodInvocationAuthorizationService.CheckAsync(
                    new MethodInvocationFeatureCheckerContext(methodInfo)
                );

                await next();
            }
        }
    }
}
