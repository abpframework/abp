﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.AspNetCore.Mvc.Features
{
    public class AbpFeaturePageFilter : IAsyncPageFilter, ITransientDependency
    {
        private readonly IMethodInvocationFeatureCheckerService _methodInvocationAuthorizationService;

        public AbpFeaturePageFilter(IMethodInvocationFeatureCheckerService methodInvocationAuthorizationService)
        {
            _methodInvocationAuthorizationService = methodInvocationAuthorizationService;
        }
        
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
        
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HandlerMethod == null || !context.ActionDescriptor.IsPageAction())
            {
                await next();
                return;
            }

            var methodInfo = context.HandlerMethod.MethodInfo;

            using (AbpCrossCuttingConcerns.Applying(context.HandlerInstance, AbpCrossCuttingConcerns.FeatureChecking))
            {
                await _methodInvocationAuthorizationService.CheckAsync(
                    new MethodInvocationFeatureCheckerContext(methodInfo)
                );

                await next();
            }
        }
    }
}