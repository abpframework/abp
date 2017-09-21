using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    public class AbpValidationActionFilter : IAsyncActionFilter, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public AbpValidationActionFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TODO: Configuration to disable validation for controllers..?
            if (!context.ActionDescriptor.IsControllerAction() || 
                !ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
            {
                await next();
                return;
            }

            using (AbpCrossCuttingConcerns.Applying(context.Controller, AbpCrossCuttingConcerns.Validation))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var validator = scope.ServiceProvider.GetRequiredService<MvcActionInvocationValidator>();
                    validator.Initialize(context);
                    validator.Validate();
                }

                await next();
            }
        }
    }
}
