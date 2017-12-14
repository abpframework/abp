using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    public class MvcActionInvocationValidationContext : MethodInvocationValidationContext
    {
        public ActionExecutingContext ActionContext { get; }
        
        public MvcActionInvocationValidationContext(ActionExecutingContext actionContext) 
            : base(actionContext.ActionDescriptor.GetMethodInfo(), GetParameterValues(actionContext))
        {
            ActionContext = actionContext;
        }

        private static object[] GetParameterValues(ActionExecutingContext actionContext)
        {
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfo();

            var parameters = methodInfo.GetParameters();
            var parameterValues = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                parameterValues[i] = actionContext.ActionArguments.GetOrDefault(parameters[i].Name);
            }

            return parameterValues;
        }
    }
}