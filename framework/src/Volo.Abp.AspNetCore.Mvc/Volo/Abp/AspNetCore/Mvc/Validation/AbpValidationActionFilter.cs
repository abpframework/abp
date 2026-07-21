using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Filters;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation;

public class AbpValidationActionFilter : IAsyncActionFilter, IAbpFilter, ITransientDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction() ||
            !context.ActionDescriptor.HasObjectResult())
        {
            await next();
            return;
        }

        if (!context.GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>().Value.AutoModelValidation)
        {
            await next();
            return;
        }

        if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(context.ActionDescriptor.GetMethodInfo()) != null)
        {
            await next();
            return;
        }

        if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(context.Controller.GetType()) != null)
        {
            await next();
            return;
        }

        var effectiveMethod = GetEffectiveMethodInfo(context);
        if (effectiveMethod != null)
        {
            if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(effectiveMethod) != null)
            {
                await next();
                return;
            }
        }

        context.GetRequiredService<IModelStateValidator>().Validate(context.ModelState);

        if (context.Controller is IValidationEnabled)
        {
            await ValidateActionArgumentsAsync(context, effectiveMethod);
        }

        await next();
    }

    protected virtual MethodInfo? GetEffectiveMethodInfo(ActionExecutingContext context)
    {
        var baseMethod = context.ActionDescriptor.GetMethodInfo();
        if (baseMethod.DeclaringType == context.Controller.GetType())
        {
            return null;
        }

        return context.Controller.GetType().GetMethods().FirstOrDefault(x =>
            x.DeclaringType == context.Controller.GetType() &&
            x.Name == baseMethod.Name &&
            x.ReturnType == baseMethod.ReturnType &&
            x.GetParameters().Select(p => p.ToString()).SequenceEqual(baseMethod.GetParameters().Select(p => p.ToString())));
    }

    protected virtual async Task ValidateActionArgumentsAsync(ActionExecutingContext context, MethodInfo? effectiveMethod = null)
    {
        var methodInfo = effectiveMethod ?? context.ActionDescriptor.GetMethodInfo();

        var parameterValues = methodInfo.GetParameters()
            .Select(p => context.ActionArguments.TryGetValue(p.Name!, out var value) ? value : null)
            .ToArray();

        await context.GetRequiredService<IMethodInvocationValidator>().ValidateAsync(
            new MethodInvocationValidationContext(
                context.Controller,
                methodInfo,
                parameterValues
            )
        );
    }
}
