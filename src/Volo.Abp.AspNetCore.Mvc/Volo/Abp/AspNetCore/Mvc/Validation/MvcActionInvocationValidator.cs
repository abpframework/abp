using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    public class MvcActionInvocationValidator : MethodInvocationValidatorBase
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public MvcActionInvocationValidator(IMvcModelObjectValidator objectValidator)
            : base(objectValidator)
        {

        }

        public virtual void Validate(MvcActionInvocationValidationContext context)
        {
            AddModelStateErrors(context);
            ValidateInternal(context);
        }

        public virtual void AddModelStateErrors(MvcActionInvocationValidationContext context)
        {
            if (context.ActionContext.ModelState.IsValid)
            {
                return;
            }

            foreach (var state in context.ActionContext.ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    context.Errors.Add(new ValidationResult(error.ErrorMessage, new[] { state.Key }));
                }
            }
        }
    }
}