using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    public class AbpValidationActionFilter : IAsyncActionFilter, ITransientDependency
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TODO: Configuration to disable validation for controllers..?

            if (!context.ActionDescriptor.IsControllerAction() ||
                !context.ActionDescriptor.HasObjectResult())
            {
                await next();
                return;
            }

            context.GetRequiredService<IModelStateValidator>().Validate(context.ModelState);
            await next();
        }
    }
}
